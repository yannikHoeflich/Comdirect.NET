using System.Text.Json.Serialization;

namespace Comdirect.NET.Models;

public record PagingObject<T>(
    [property: JsonPropertyName("paging")]
    Paging Paging,
    [property: JsonPropertyName("values")]
    IReadOnlyCollection<T> Values
    );