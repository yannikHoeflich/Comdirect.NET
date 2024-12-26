using System.Text.Json.Serialization;
using Comdirect.NET.Models.Values;

namespace Comdirect.NET.Models.Instruments;

public record FundDistribution(
    [property: JsonPropertyName("currency")]
    string Currency,
    [property: JsonPropertyName("regularIssueSurcharge")]
    AmountValue RegularIssueSurcharge,
    [property: JsonPropertyName("discountIssueSurcharge")]
    AmountValue DiscountIssueSurcharge,
    [property: JsonPropertyName("reducedIssueSurcharge")]
    AmountValue ReducedIssueSurcharge,
    [property: JsonPropertyName("investmentCategory")]
    string InvestmentCategory,
    [property: JsonPropertyName("totalExpenseRatio")]
    AmountValue TotalExpenseRatio,
    [property: JsonPropertyName("rating")]
    Rating? Rating
    );