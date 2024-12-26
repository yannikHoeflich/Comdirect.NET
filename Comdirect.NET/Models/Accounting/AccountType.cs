using System.Text.Json.Serialization;

namespace Comdirect.NET.Models.Accounting;

public enum AccountTypeShort {
    [JsonStringEnumMemberName("FX")]
    Foreign,
    [JsonStringEnumMemberName("OF")]
    Option,
    [JsonStringEnumMemberName("CA")]
    Checking,
    [JsonStringEnumMemberName("DAS")]
    DirectAccessSaving,
    [JsonStringEnumMemberName("CDF")]
    ContractForDifference,
    [JsonStringEnumMemberName("SA")]
    Settlement,
    [JsonStringEnumMemberName("LLA")]
    LombardLoan
}

public class AccountType : IFakeEnum<AccountTypeShort>, IEquatable<AccountType> {
    public bool Equals(AccountType? other) {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Short == other.Short;
    }

    public override bool Equals(object? obj) {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((AccountType)obj);
    }

    public override int GetHashCode() {
        return (int)Short;
    }

    public static bool operator ==(AccountType? left, AccountType? right) {
        return Equals(left, right);
    }

    public static bool operator !=(AccountType? left, AccountType? right) {
        return !Equals(left, right);
    }

    public AccountType() {
        Description = "";
    }

    public AccountType(AccountTypeShort s, string description) {
        Short = s;
        Description = description;
    }
    
    public static readonly AccountType Foreign = new(AccountTypeShort.Foreign, "Foreign");
    public static readonly AccountType Option = new(AccountTypeShort.Option, "Option");
    public static readonly AccountType Checking = new(AccountTypeShort.Checking, "Checking");
    public static readonly AccountType DirectAccessSaving = new(AccountTypeShort.DirectAccessSaving, "DirectAccessSaving");
    public static readonly AccountType ContractForDifference = new(AccountTypeShort.ContractForDifference, "ContractForDifference");
    public static readonly AccountType Settlement = new(AccountTypeShort.Settlement, "Settlement");
    public static readonly AccountType LombardLoan = new(AccountTypeShort.LombardLoan, "LombardLoan");

    [JsonPropertyName("key"), JsonConverter(typeof(JsonStringEnumConverter<AccountTypeShort>))]
    public AccountTypeShort Short { get; set; }
    
    [JsonPropertyName("text")]
    public string Description { get; set; }
}