using System.Text.Json.Serialization;
using Comdirect.NET.Models.Accounting;

namespace Comdirect.NET.Models;

public class TransactionType : IFakeEnum<TransactionTypeShort>, IEquatable<TransactionType> {
    public bool Equals(TransactionType? other) {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Short == other.Short;
    }

    public override bool Equals(object? obj) {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TransactionType)obj);
    }

    public override int GetHashCode() {
        return (int)Short;
    }

    public static bool operator ==(TransactionType? left, TransactionType? right) {
        return Equals(left, right);
    }

    public static bool operator !=(TransactionType? left, TransactionType? right) {
        return !Equals(left, right);
    }

    public TransactionType(TransactionTypeShort s, string description) {
        Short = s;
        Description = description;
    }
    
    public TransactionType() {
        Description = "";
    }

    [JsonPropertyName("key"), JsonConverter(typeof(JsonStringEnumConverter<TransactionTypeShort>))]
    public TransactionTypeShort Short { get; set; }
    
    [JsonPropertyName("text")]
    public string Description { get; set; }
}

public enum TransactionTypeShort {
    [JsonStringEnumMemberName("Sparplan")]
    SavingPlan,

    [JsonStringEnumMemberName("Wertpapier")]
    Securities,

    [JsonStringEnumMemberName("Geldanlage")]
    InvestmentSaving,

    [JsonStringEnumMemberName("Bankgebühren")]
    BankFees,

    [JsonStringEnumMemberName("Sonstiges")]
    Miscellaneous,

    [JsonStringEnumMemberName("Bar")]
    Cash,

    [JsonStringEnumMemberName("Zinsen / Dividenden")]
    Interest,

    [JsonStringEnumMemberName("Devisen")]
    CurrencyExchange,

    [JsonStringEnumMemberName("Storno")]
    Cancellation,

    [JsonStringEnumMemberName("Scheck")]
    Cheque,

    [JsonStringEnumMemberName("Lastschrift")]
    DirectDebit,

    [JsonStringEnumMemberName("TRANSFER")]
    Transfer,

    [JsonStringEnumMemberName("Kartenverfügung")]
    CardTransaction,

    [JsonStringEnumMemberName("Sorten (Kasse)")]
    ForeignCurrencyExchange,

    [JsonStringEnumMemberName("Geldautomat")]
    Atm,

    [JsonStringEnumMemberName("Geldanlage")]
    Savings,

    [JsonStringEnumMemberName("Dauerauftrag")]
    StandingOrder,
}