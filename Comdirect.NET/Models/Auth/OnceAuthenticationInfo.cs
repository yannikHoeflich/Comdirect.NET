using System.Text.Json.Serialization;

namespace Comdirect.NET.Models.Auth;

public record OnceAuthenticationInfo(
    [property: JsonPropertyName("id")]
    string Id,
    [property: JsonPropertyName("typ")]
    string Type,
    [property: JsonPropertyName("challenge")]
    string? Challenge,
    [property: JsonPropertyName("availableTypes")]
    string[] AvailableTypes,
    [property: JsonPropertyName("link")]
    OnceAuthenticationInfoLink Link
    );
    
    public record OnceAuthenticationInfoLink(
        [property: JsonPropertyName("href")]
        string Url,
        [property: JsonPropertyName("rel")]
        string Rel,
        [property: JsonPropertyName("method")]
        string Method,
        [property: JsonPropertyName("type")]
        string MimeType
        );