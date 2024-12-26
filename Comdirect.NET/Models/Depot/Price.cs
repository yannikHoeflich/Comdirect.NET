using System.Text.Json.Serialization;
using Comdirect.NET.Models.Values;

namespace Comdirect.NET.Models.Depot;

public record Price(
    [property: JsonPropertyName("price")]
    AmountValue Value,
    [property: JsonPropertyName("quantity")]
    AmountValue? Quantity,
    [property: JsonPropertyName("priceDateTime")]
    DateTime DateTime,
    [property: JsonPropertyName("type")] string Type
);