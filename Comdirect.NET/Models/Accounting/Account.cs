using System.Text.Json.Serialization;
using Comdirect.NET.Models.Values;

namespace Comdirect.NET.Models.Accounting;

public record Account(
    [property: JsonPropertyName("accountId")]
    string Id,
    [property: JsonPropertyName("accountDisplayId")]
    string DisplayId,
    [property: JsonPropertyName("currency")]
    string Currency,
    [property: JsonPropertyName("clientId")]
    string ClientId,
    [property: JsonPropertyName("accountType")]
    AccountType Type,
    [property: JsonPropertyName("iban")]
    string Iban,
    [property: JsonPropertyName("creditLimit")]
    AmountValue? CreditLimit
    );