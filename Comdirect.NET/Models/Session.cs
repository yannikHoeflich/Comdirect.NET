using System.Text.Json.Serialization;

namespace Comdirect.NET.Models;

public record Session(
    [property: JsonPropertyName("identifier")]
    string Id,
    [property: JsonPropertyName("sessionTanActive")]
    bool SessionTanActive,
    [property: JsonPropertyName("activated2FA")]
    bool Activated2FA);