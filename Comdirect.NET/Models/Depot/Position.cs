using System.Text.Json.Serialization;
using Comdirect.NET.Models.Instruments;
using Comdirect.NET.Models.Values;

namespace Comdirect.NET.Models.Depot;

public record Position(
    [property: JsonPropertyName("positionId")]
    string Id,
    [property: JsonPropertyName("depotId")]
    string DepotId,
    [property: JsonPropertyName("wkn")]
    string Wkn,
    [property: JsonPropertyName("custodyType")]
    string CustodyType,
    [property: JsonPropertyName("quantity")]
    AmountValue Quantity,
    [property: JsonPropertyName("availableQuantity")]
    AmountValue AvailableQuantity,
    [property: JsonPropertyName("currentPrice")]
    Price CurrentPrice,
    [property: JsonPropertyName("purchasePrice")]
    AmountValue? PurchasePrice,
    [property: JsonPropertyName("prevDayPrice")]
    Price? PreviousDayPrice,
    [property: JsonPropertyName("purchaseValue")]
    AmountValue? PurchaseValue,
    [property: JsonPropertyName("profitLossPurchaseAbs")]
    AmountValue? ProfitLossPurchaseAbs,
    [property: JsonPropertyName("profitLossPurchaseRel")]
    AmountValue? ProfitLossPurchaseRel,
    [property: JsonPropertyName("profitLossPrevDayAbs")]
    AmountValue? ProfitLossPrevDayAbs,
    [property: JsonPropertyName("profitLossPrevDayRel")]
    AmountValue? ProfitLossPrevDayRel,
    [property: JsonPropertyName("instrument")]
    Instrument Instrument
    );