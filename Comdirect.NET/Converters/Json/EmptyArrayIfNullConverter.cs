using System.Text.Json;
using System.Text.Json.Serialization;

namespace Comdirect.NET.Converters.Json;

public class EmptyArrayIfNullConverter<T> : JsonConverter<IReadOnlyCollection<T>>
{
    public override IReadOnlyCollection<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return [];
        }

        return JsonSerializer.Deserialize<List<T>>(ref reader, options)??[];
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyCollection<T>? value, JsonSerializerOptions? options)
    {
        if (value is null)
        {
            writer.WriteStartArray();
            writer.WriteEndArray();
        }
        else
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}