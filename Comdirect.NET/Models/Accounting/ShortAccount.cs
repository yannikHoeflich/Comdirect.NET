using System.Text.Json.Serialization;

namespace Comdirect.NET.Models.Accounting;

public class ShortAccount {
    [JsonPropertyName("holderName")]
    public string? HolderName { get; set; }
}