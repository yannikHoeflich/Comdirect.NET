using System.Text.Json.Serialization;
using Comdirect.NET.Models.Values;

namespace Comdirect.NET.Models.Accounting;

public record AccountBalance(
    [property: JsonPropertyName("account")]
    Account Account,
    [property: JsonPropertyName("accountId")]
    string Id,
    [property: JsonPropertyName("balance")]
    AmountValue Balance,
    [property: JsonPropertyName("balanceEUR")]
    AmountValue BalanceEur,
    [property: JsonPropertyName("availableCashAmount")]
    AmountValue AvailableCash,
    [property: JsonPropertyName("availableCashAmountEUR")]
    AmountValue AvailableCashEur
    );