using System.Text.Json.Serialization;

namespace Comdirect.NET.Models.Auth;

public record LoginRequest(
    [property: JsonPropertyName("client_id")]
    string ClientId,
    [property: JsonPropertyName("client_secret")]
    string ClientSecret,
    [property: JsonPropertyName("username")]
    string Username,
    [property: JsonPropertyName("password")]
    string Password,
    [property: JsonPropertyName("grant_type")]
    string GrantType
    );