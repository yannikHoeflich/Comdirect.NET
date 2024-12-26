using System.Text.Json;
using System.Text.Json.Serialization;

namespace Comdirect.NET.Converters.Json;

public class StringToInt : JsonConverter<int> {
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        return int.Parse(reader.ValueSpan);
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.ToString());
    }
}