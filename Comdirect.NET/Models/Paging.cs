using System.Text.Json.Serialization;

namespace Comdirect.NET.Models;

public record Paging(
    [property: JsonPropertyName("index")]
    int Index,
    [property: JsonPropertyName("matches")]
    int Count
    );