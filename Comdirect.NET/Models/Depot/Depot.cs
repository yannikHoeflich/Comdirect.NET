using System.Text.Json.Serialization;
using Comdirect.NET.Converters.Json;

namespace Comdirect.NET.Models.Depot;

public record Depot(
    [property: JsonPropertyName("depotId")]
    string Id,
    [property: JsonPropertyName("depotDisplayId")]
    string DisplayId,
    [property: JsonPropertyName("clientId")]
    string ClientId,
    [property: JsonPropertyName("defaultSettlementAccountId")]
    string DefaultSettlementAccountId,
    [property: JsonPropertyName("settlementAccountIds"), JsonConverter(typeof(EmptyArrayIfNullConverter<string>))]
    IReadOnlyCollection<string> SettlementAccountIds
    );