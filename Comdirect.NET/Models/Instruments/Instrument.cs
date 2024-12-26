using System.Text.Json.Serialization;

namespace Comdirect.NET.Models.Instruments;

public record Instrument(
    [property: JsonPropertyName("instrumentId")]
    string Id,
    [property: JsonPropertyName("wkn")]
    string Wkn,
    [property: JsonPropertyName("mnemonic")]
    string Mnemonic,
    [property: JsonPropertyName("isin")]
    string Isin,
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("shortName")]
    string ShortName,
    [property: JsonPropertyName("staticData")]
    StaticData? StaticData,
    [property: JsonPropertyName("orderDimensions")]
    Dimensions? OrderDimensions,
    [property: JsonPropertyName("fundsDistribution")]
    FundDistribution? FundDistribution,
    [property: JsonPropertyName("derivativeData")]
    DerivativeData? DerivativeData
    );