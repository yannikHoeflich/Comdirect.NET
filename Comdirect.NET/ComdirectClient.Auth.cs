using System.Collections.Immutable;
using System.Text.Json;
using Comdirect.NET.Extensions;
using Comdirect.NET.Models;
using Comdirect.NET.Models.Auth;
using FluentResults;

namespace Comdirect.NET;

public partial class ComdirectClient {
    public void ConfigureTanPrioritization(IReadOnlyCollection<TanType> prioritization) {
        _tanTypePrioritization = prioritization.Distinct().ToImmutableArray();
    }

    public async Task<Result> Login(string username, string password) {
        var loginRequest = new Dictionary<string, string>() {
            {"client_id", _clientId},
            {"client_secret", _clientSecret},
            {"username", username},
            {"password", password},
            {"grant_type", "password"},
        };
        Result<TokenResponse> response
            = await Request<TokenResponse, Dictionary<string, string>>(HttpMethod.Post, "oauth/token", loginRequest);
        if (response.IsFailed) {
            return Result.Fail(response.Errors);
        }

        TokenResponse? tokenResponse = response.Value;
        var token = new Token(tokenResponse.AccessToken, tokenResponse.RefreshToken,
                              Environment.TickCount64 + tokenResponse.ExpiresIn * 1000 - 2000);
        _token = token;
        return Result.Ok();
    }

    public async Task<Result<Session>> GetSession() {
        var sessions = await Request<Session[]>(HttpMethod.Get, $"api/session/clients/{_clientId}/v1/sessions");
        if (sessions.IsFailed) {
            return sessions.ToResult();
        }

        if (sessions.Value.Length < 1) {
            return Result.Fail("The API returned an empty session array");
        }

        return sessions.Value[0];
    }
    
    public async Task<Result<OnceAuthenticationInfo>> ValidateTan(Session session) {
        return await RequestFromHeader<OnceAuthenticationInfo, Session>(HttpMethod.Post,
                                               $"api/session/clients/{_clientId}/v1/sessions/{session.Id}/validate", "x-once-authentication-info",
                                               session);
    }

    public async Task<Result<OnceAuthenticationInfo>> ValidateTan(Session session, TanType tanType) {
        Dictionary<string, string> headers = new Dictionary<string, string> {{"x-once-authentication-info", $$"""{"typ":"{{tanType.Name}}"}"""}};
        return await RequestFromHeader<OnceAuthenticationInfo, Session>(HttpMethod.Post,
                                                                        $"api/session/clients/{_clientId}/v1/sessions/{session.Id}/validate", "x-once-authentication-info",
                                                                        session, headers);
    }


    public async Task<Result<bool>> CheckTan(Tan tan) {
        if (tan.Link is null) {
            return false;
        }
        
        Result<TanStatus> result = await Request<TanStatus>(HttpMethod.Get, tan.Link);

        if (result.IsFailed) {
            return result.ToResult();
        }
        
        return result.Value.Status == AuthenticationStatus.Authenticated;
    }

    public async Task<Result<Session>> ActivateTan(Session session, Tan tan, string? confirmedChallenge) {
        if (tan.Type == TanType.Push && !string.IsNullOrEmpty(confirmedChallenge)) {
            return Result.Fail("The tan type is push, so 'confirmedChallenge' should be empty");
        }

        if (tan.Type != TanType.Push && string.IsNullOrEmpty(confirmedChallenge)) {
            return Result.Fail("The tan type is not push, so 'confirmedChallenge' is required");
        }

        var headers = new Dictionary<string, string>(2);
        headers.Add("x-once-authentication-info", JsonSerializer.Serialize(new IdObject(tan.Id)));
        if (!string.IsNullOrEmpty(confirmedChallenge)) {
            headers.Add("x-once-authentication", confirmedChallenge);
        }
        
        var result = await Request<Session, Session>(HttpMethod.Patch, $"api/session/clients/{_clientId}/v1/sessions/{session.Id}", session, headers);
        if (result.IsSuccess) {
            _tan = tan;
        }

        return result;
    }

    public async Task<Result> SwapTokens() {
        if (_token is null) {
            return Result.Fail("Please login before swapping the tokens");
        }

        var content = new Dictionary<string, string>() {
            { "client_id", _clientId },
            { "client_secret", _clientSecret },
            { "grant_type", "cd_secondary" },
            { "token", _token.Value.AccessToken },
        };

        Result<TokenResponse> response = await Request<TokenResponse, Dictionary<string, string>>(HttpMethod.Post, "oauth/token", content);

        if (response.IsFailed) {
            return response.ToResult();
        }

        TokenResponse? tokenResponse = response.Value;
        
        var newToken = new Token(tokenResponse.AccessToken, tokenResponse.RefreshToken,
                                 Environment.TickCount64 + tokenResponse.ExpiresIn * 1000 - 2000);

        _token = newToken;

        return Result.Ok();
    }

    public async Task<Result<Tan>> LoginUntilTan(string username, string password) {
        Result tokenResult = await Login(username, password);
        if (tokenResult.IsFailed) {
            return tokenResult;
        }

        Result<Session> sessionResult = await GetSession();
        if (sessionResult.IsFailed) {
            return sessionResult.ToResult();
        }

        _sessionId = new SessionId(sessionResult.Value.Id);

        var session = sessionResult.Value with { SessionTanActive = true, Activated2FA = true };
        
        Result<OnceAuthenticationInfo> validationInfo = await ValidateTan(session);
        if (validationInfo.IsFailed) {
            return validationInfo.ToResult();
        }

        foreach (TanType t in _tanTypePrioritization.Where(t => validationInfo.Value.AvailableTypes.Contains(t.Name))) {
            if (validationInfo.Value.Type == t.Name) {
                continue;
            }
            validationInfo = await ValidateTan(session, t);
            if (validationInfo.IsFailed) {
                return validationInfo.ToResult();
            }
            break;
        }

        if (!TanType.TryParse(validationInfo.Value.Type, out TanType tanType)) {
            return Result.Fail("API returned unsupported Tan type");
        }

        return new Tan(validationInfo.Value.Id, tanType, validationInfo.Value.Challenge);
    }
    
    public async Task<Result> FullLogin(string username, string password, CancellationToken? cancellationToken = null) {
        Result tokenResult = await Login(username, password);
        if (tokenResult.IsFailed) {
            return tokenResult;
        }

        Result<Session> sessionResult = await GetSession();
        if (sessionResult.IsFailed) {
            return sessionResult.ToResult();
        }
        var session = sessionResult.Value with { SessionTanActive = true, Activated2FA = true };

        Result<OnceAuthenticationInfo> validationInfo = await ValidateTan(session, TanType.Push);
        if (validationInfo.IsFailed) {
            return validationInfo.ToResult();
        }

        var tan = new Tan(validationInfo.Value.Id, TanType.Push, validationInfo.Value.Challenge) {
            Link = validationInfo.Value.Link.Url
        };

        const int checkFrequencySeconds = 3;
        const int timeoutSeconds = 180;
        const int checkCount = timeoutSeconds / checkFrequencySeconds;
        Result<bool> checkResult = Result.Fail("Something failed horrible");
        for (int i = 0; i < checkCount && !cancellationToken.IsCancelled(); i++) {
            checkResult = await CheckTan(tan);
            if (checkResult.IsSuccess && checkResult.Value) {
                break;
            }

            if (cancellationToken is not null) {
                await Task.Delay(checkFrequencySeconds * 1000, cancellationToken.Value);
            } else {
                await Task.Delay(checkFrequencySeconds * 1000);
            }
        }

        if (cancellationToken.IsCancelled()) {
            return Result.Fail("Cancellation token was canceled");
        }
        
        if (!checkResult.IsSuccess) {
            return checkResult.ToResult();
        }

        Result<Session> tanResult = await ActivateTan(session, tan, null);
        if (tanResult.IsFailed) {
            return tanResult.ToResult();
        }

        return await SwapTokens();
    }

    private async Task<Result> RefreshToken() {
        if (_token is null) {
            return Result.Fail("Please login before refreshing the token");
        }
        var request = new Dictionary<string, string>() {
            {"client_id", _clientId},
            {"client_secret", _clientSecret},
            {"grant_type", "refresh_token"},
            {"refresh_token", _token.Value.RefreshToken}
        };
        Result<RefreshResponse> response = await Request<RefreshResponse, Dictionary<string, string>>(HttpMethod.Post, "oauth/token", request);

        if (response.IsFailed) {
            return response.ToResult();
        }

        RefreshResponse tokenResponse = response.Value;

        var newToken = new Token(tokenResponse.AccessToken, tokenResponse.RefreshToken,
                                 Environment.TickCount64 + tokenResponse.ExpiresIn * 1000 - 2000);

        _token = newToken;
        
        return Result.Ok();
    }

    public void KeepAlive(CancellationToken? cancellationToken = null) {
        Task.Run(async () => {
            while (!_token.HasValue) {
                await Task.Delay(100);
            }

            while (!cancellationToken.IsCancelled()) {
                long ticksUntilCancellation = Environment.TickCount64 - _token.Value.ExpiresAtTick;
                TimeSpan timeUntilRefresh = TimeSpan.FromMilliseconds(ticksUntilCancellation) - TimeSpan.FromSeconds(10);
                if (cancellationToken is null) {
                    await Task.Delay(timeUntilRefresh);
                } else {
                    await Task.Delay(timeUntilRefresh, cancellationToken.Value);
                }

                if (!cancellationToken.IsCancelled()) {
                    await RefreshToken();
                }
            }
        });
    }
}