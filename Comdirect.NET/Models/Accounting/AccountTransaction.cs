using System.Text.Json.Serialization;
using Comdirect.NET.Models.Values;

namespace Comdirect.NET.Models.Accounting;

public record AccountTransaction(
    [property: JsonPropertyName("bookingStatus"), JsonConverter(typeof(JsonStringEnumConverter<BookingStatus>))]
    BookingStatus BookingStatus,
    [property: JsonPropertyName("bookingDate")]
    DateOnly? Date,
    [property: JsonPropertyName("amount")]
    AmountValue Amount,
    [property: JsonPropertyName("remitter")]
    ShortAccount? Remitter,
    [property: JsonPropertyName("debtor")]
    ShortAccount? Debtor,
    [property: JsonPropertyName("creditor")]
    ShortAccount? Creditor,
    [property: JsonPropertyName("reference")]
    string Reference,
    [property: JsonPropertyName("endToEndReference")]
    string EndToEndReference,
    [property: JsonPropertyName("valutaDate")]
    DateOnly ValutaDate,
    [property: JsonPropertyName("directDebitCreditorId")]
    string DirectDebitCreditorId,
    [property: JsonPropertyName("directDebitMandateId")]
    string DirectDebitMandateId,
    [property: JsonPropertyName("transactionType")]
    TransactionType TransactionType,
    [property: JsonPropertyName("remittanceInfo")]
    string RemittanceInfo,
    [property: JsonPropertyName("newTransaction")]
    bool NewTransaction
);
