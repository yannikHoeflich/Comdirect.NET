using System.Text.Json.Serialization;
using Comdirect.NET.Models.Depot;
using Comdirect.NET.Models.Values;

namespace Comdirect.NET.Models.Instruments;

public record DerivativeData(
    [property: JsonPropertyName("underlyingInstrument")]
    Instrument? Instrument,
    [property: JsonPropertyName("underlyingPrice")]
    Price? Price,
    [property: JsonPropertyName("certificateType")]
    string? CertificateType,
    [property: JsonPropertyName("rating")]
    Rating? Rating,
    [property: JsonPropertyName("strikePrice")]
    AmountValue StrikePrice,
    [property: JsonPropertyName("leverage")]
    string? Leverage,
    [property: JsonPropertyName("multiplier")]
    string? Multiplier,
    [property: JsonPropertyName("expiryDate")]
    DateOnly? ExpiryDate,
    [property: JsonPropertyName("yieldPA")]
    AmountValue? YieldPa,
    [property: JsonPropertyName("remainingTermInYears")]
    AmountValue? RemainingTermInYears,
    [property: JsonPropertyName("nominalRate")]
    AmountValue? NominalRate,
    [property: JsonPropertyName("warrantType")]
    string? WarrantType,
    [property: JsonPropertyName("maturityDate")]
    DateOnly? MaturityDate,
    [property: JsonPropertyName("interestPaymentDate")]
    DateOnly? InterestPaymentDate,
    [property: JsonPropertyName("interestPaymentInterval")]
    string? InterestPaymentInterval,
    [property: JsonPropertyName("underlyingInstrument")]
    Instrument? UnderlyingInstrument
    );