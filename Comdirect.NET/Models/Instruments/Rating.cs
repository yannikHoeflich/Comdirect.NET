using System.Text.Json.Serialization;

namespace Comdirect.NET.Models.Instruments;

public record Rating(
    [property: JsonPropertyName("morningstar")]
    string? Morningstar,
    [property: JsonPropertyName("moodys")]
    string? Moodys
    );