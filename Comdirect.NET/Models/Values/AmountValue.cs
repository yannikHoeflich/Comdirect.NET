using System.Text.Json.Serialization;
using Comdirect.NET.Converters.Json;

namespace Comdirect.NET.Models.Values;

public record struct AmountValue(
    [property: JsonPropertyName("value"), JsonConverter(typeof(AmountStringToDouble))]
    double Value,
    [property: JsonPropertyName("unit")]
    string Unit
    );