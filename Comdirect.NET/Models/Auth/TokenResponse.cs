using System.ComponentModel;
using System.Text.Json.Serialization;
using Comdirect.NET.Converters.Json;

namespace Comdirect.NET.Models.Auth;

public record TokenResponse(
    [property: JsonPropertyName("access_token")]
    string AccessToken,
    [property: JsonPropertyName("token_type")]
    string TokenType,
    [property: JsonPropertyName("refresh_token")]
    string RefreshToken,
    [property: JsonPropertyName("expires_in")]
    int ExpiresIn,
    [property: JsonPropertyName("scope")]
    string Scope,
    [property: JsonPropertyName("kdnr"), JsonConverter(typeof(IntToString))]
    string CustomerId,
    [property: JsonPropertyName("bpid"), JsonConverter(typeof(IntToString))]
    string Id,
    [property: JsonPropertyName("kontaktId"), JsonConverter(typeof(IntToString))]
    string ContactId
    );