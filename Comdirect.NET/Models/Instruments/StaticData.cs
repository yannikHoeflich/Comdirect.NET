using System.Text.Json.Serialization;

namespace Comdirect.NET.Models.Instruments;

public record StaticData(
    [property: JsonPropertyName("notation")]
    string Notation,
    [property: JsonPropertyName("currency")]
    string Currency,
    [property: JsonPropertyName("instrumentType"), JsonConverter(typeof(JsonStringEnumConverter<InstrumentType>))]
    InstrumentType InstrumentType,
    [property: JsonPropertyName("priipsRelevant")]
    bool PriipsRelevant,
    [property: JsonPropertyName("kidAvailable")]
    bool KidAvailable,
    [property: JsonPropertyName("shippingWaiverRequired")]
    bool ShippingWaiverRequired,
    [property: JsonPropertyName("fundRedemptionLimited")]
    bool FundRedemptionLimited
    );

public enum InstrumentType {
    [JsonStringEnumMemberName("SHARE")]
    Share,
    [JsonStringEnumMemberName("BONDS")]
    Bond,
    [JsonStringEnumMemberName("SUBSCRIPTION_RIGHT")]
    SubscriptionRight,
    [JsonStringEnumMemberName("ETF")]
    Etf,
    [JsonStringEnumMemberName("PROFIT_PART_CERTIFICATE")]
    ProfitPartCertificate,
    [JsonStringEnumMemberName("FUND")]
    Fund,
    [JsonStringEnumMemberName("WARRANT")]
    Warrant,
    [JsonStringEnumMemberName("CERTIFICATE")]
    Certificate,
    [JsonStringEnumMemberName("NOT_AVAILABLE")]
    NotAvailable
}