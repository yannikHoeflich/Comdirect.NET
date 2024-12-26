using System.Text.Json;
using System.Text.Json.Serialization;

namespace Comdirect.NET.Converters.Json;

public class IntToString : JsonConverter<string> {
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        return reader.ValueSpan.ToString();
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) {
        writer.WriteStringValue(value);
    }
}