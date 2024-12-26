using System.Text.Json.Serialization;
using Comdirect.NET.Models.Accounting;
using Comdirect.NET.Models.Instruments;
using Comdirect.NET.Models.Values;

namespace Comdirect.NET.Models.Depot;

public record DepotTransaction(
    [property: JsonPropertyName("transactionId")]
    string Id,
    [property: JsonPropertyName("bookingStatus"), JsonConverter(typeof(JsonStringEnumConverter<BookingStatus>))]
    BookingStatus BookingStatus,
    [property: JsonPropertyName("bookingDate")]
    DateOnly? BookingDate,
    [property: JsonPropertyName("businessDate")]
    DateOnly BusinessDate,
    [property: JsonPropertyName("quantity")]
    AmountValue Quantity,
    [property: JsonPropertyName("instrumentId")]
    string InstrumentId,
    [property: JsonPropertyName("instrument")]
    Instrument? Instrument,
    [property: JsonPropertyName("executionPrice")]
    AmountValue ExecutionPrice,
    [property: JsonPropertyName("transactionValue")]
    AmountValue TransactionValue,
    [property: JsonPropertyName("transactionDirection"), JsonConverter(typeof(JsonStringEnumConverter<TransactionDirection>))]
    TransactionDirection TransactionDirection,
    [property: JsonPropertyName("transactionDirection"), JsonConverter(typeof(JsonStringEnumConverter<DepotTransactionType>))]
    DepotTransactionType TransactionType
    );

public enum DepotTransactionType {
    [JsonStringEnumMemberName("SELL")]
    Sell,
    [JsonStringEnumMemberName("BUY")]
    Buy,
    [JsonStringEnumMemberName("OTHER")]
    Other,
    [JsonStringEnumMemberName("TRANSFER_IN")]
    TransferIn,
    [JsonStringEnumMemberName("TRANSFER_OUT")]
    TransferOut
}

public enum TransactionDirection {
    [JsonStringEnumMemberName("IN")]
    In, 
    [JsonStringEnumMemberName("OUT")]
    Out
}