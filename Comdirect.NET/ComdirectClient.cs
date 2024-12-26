using System.Collections.Immutable;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Comdirect.NET.Extensions;
using Comdirect.NET.Models;
using Comdirect.NET.Models.Auth;
using FluentResults;

namespace Comdirect.NET;

public partial class ComdirectClient {
    private readonly HttpClient _httpClient;
    private SessionId _sessionId = SessionId.Generate();

    private readonly string _clientId;
    private readonly string _clientSecret;

    private Token? _token = null;

    private ImmutableArray<TanType> _tanTypePrioritization = [TanType.Push, TanType.Phone, TanType.Graphic];

    private Tan? _tan = null;

    public ComdirectClient(string clientId, string clientSecret) {
        _clientId = clientId;
        _clientSecret = clientSecret;
        _httpClient = new HttpClient { BaseAddress = new Uri("https://api.comdirect.de/") };
    }

    private async Task<Result<TOut>> Request<TOut>(HttpMethod method,
        string module,
        int version,
        string resource,
        string? id,
        string? information)
        => await Request<TOut, object>(method, module, version, resource, id, information, null);

    private async Task<Result<TOut>> Request<TOut, TIn>(HttpMethod method,
        string module,
        int version,
        string resource,
        string? id,
        string? information,
        TIn? content) {
        var url = new StringBuilder($"api/{module}/v{version}/{resource}");

        if (id is not null) {
            url.Append('/');
            url.Append(id);
        }
        
        if (information is not null) {
            url.Append('/');
            url.Append(information);
        }
        
        return await Request<TOut, TIn>(method, url.ToString(), content, null);
    }

    private async Task<Result<TOut>> Request<TOut>(HttpMethod method, string url)
        => await Request<TOut, object>(method, url, null, null);

    private async Task<Result<TOut>> Request<TOut, TIn>(HttpMethod method,
        string url,
        TIn? content,
        Dictionary<string, string>? headers = null) {
        await RefreshIfRequired();
        var request = new HttpRequestMessage(method, url);
        var requestInfo = new RequestInfo(RequestId.Generate(_sessionId));
        request.Headers.TryAddWithoutValidation("x-http-request-info", requestInfo.ToJson());

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        if (_token is not null) {
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {_token.Value.AccessToken}");
        }

        if (_tan is not null) {
            // request.Headers.TryAddWithoutValidation("x-http-request-info", JsonSerializer.Serialize(new IdObject(_tan.Id)));
        }
        
        if (headers is not null) {
            foreach (var header in headers) {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        if (content is not null) {
            if (content is Dictionary<string, string> dictionary) {
                request.Content = new FormUrlEncodedContent(dictionary);
            } else {
                request.Content = JsonContent.Create(content);
            }
        }

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode) {
            string responseMessage = await response.Content.ReadAsStringAsync();
            return Result.Fail($"Api returned: {responseMessage}");
        }

        var responseString = await response.Content.ReadAsStringAsync();
        TOut? responseObject;
        try {
            responseObject = JsonSerializer.Deserialize<TOut>(responseString);
            // responseObject = await response.Content.ReadFromJsonAsync<TOut>();
        } catch (Exception ex) {
            return Result.Fail(new Error(ex.Message).CausedBy(ex));
        }

        if (responseObject is null) {
            return Result.Fail("The Api returned null");
        }

        return responseObject;
    }

    private async Task<Result<TOut>> RequestFromHeader<TOut, TIn>(HttpMethod method,
        string url,
        string headerName,
        TIn? content,
        Dictionary<string, string>? headers = null) {
        await RefreshIfRequired();

        var request = new HttpRequestMessage(method, url);
        var requestInfo = new RequestInfo(RequestId.Generate(_sessionId));
        request.Headers.TryAddWithoutValidation("x-http-request-info", requestInfo.ToJson());

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        if (_token is not null) {
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {_token.Value.AccessToken}");
        }

        if (_tan is not null) {
            request.Headers.TryAddWithoutValidation("x-http-request-info", JsonSerializer.Serialize(new IdObject(_tan.Id)));
        }

        if (headers is not null) {
            foreach (var header in headers) {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }


        if (content is not null) {
            if (content is Dictionary<string, string> dictionary) {
                request.Content = new FormUrlEncodedContent(dictionary);
            } else {
                request.Content = JsonContent.Create(content);
            }
        }

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode) {
            string responseMessage = await response.Content.ReadAsStringAsync();
            return Result.Fail(responseMessage);
        }


        if (!response.Headers.TryGetValues(headerName, out IEnumerable<string>? values)) {
            return Result.Fail("Invalid response from API, no header 'x-once-authentication-info'");
        }

        string? responseString = values.FirstOrDefault();
        if (responseString is null) {
            return Result.Fail("Invalid response from API, no header 'x-once-authentication-info'");
        }

        TOut? responseObject;
        try {
            responseObject = JsonSerializer.Deserialize<TOut>(responseString);
        } catch (Exception ex) {
            return Result.Fail(new Error(ex.Message).CausedBy(ex));
        }

        if (responseObject is null) {
            return Result.Fail("The Api returned null");
        }

        return responseObject;
    }

    private async ValueTask RefreshIfRequired() {
        if (_token is null) {
            return;
        }

        if (!_token.Value.IsExpired) {
            return;
        }

        await RefreshToken();
    }
}