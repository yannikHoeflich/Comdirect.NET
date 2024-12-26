using System.Text.Json.Serialization;

namespace Comdirect.NET.Models.Auth;

public record RefreshTokenRequest(
    [property: JsonPropertyName("client_id")]
    string ClientId,
    [property: JsonPropertyName("client_secret")]
    string ClientSecret,
    [property: JsonPropertyName("refresh_token")]
    string RefreshToken,
    [property: JsonPropertyName("grant_type")]
    string GrantType = "refresh_token"
    );