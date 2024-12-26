using System.Text.Json.Serialization;

namespace Comdirect.NET.Models;

public record IdObject(
    [property: JsonPropertyName("id")]
    string Id
    );