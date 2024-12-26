using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Comdirect.NET.Converters.Json;

public class AmountStringToDouble : JsonConverter<double> {
    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        string valueString = Encoding.UTF8.GetString(reader.ValueSpan);
        ReadOnlySpan<char> value = valueString;
        var splitted = value.Split('+');
        int i = 0;
        double result = 0;
        foreach (Range range in splitted) {
            if (i > 1) {
                break;
            }
            ReadOnlySpan<char> valueSpan = value[range];

            int valueInt = int.Parse(valueSpan);

            if (i == 0) {
                result = valueInt;
            } else {
                result += valueInt / double.Pow(10, Math.Ceiling(double.Log10(valueInt)));
            }
            
            i++;
        }
        return result;
    }

    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options) {
        StringBuilder builder = new StringBuilder();

        int intValue = (int)value;
        double decimalValueDouble = value - intValue;
        int decimalValue = (int)Math.Round(decimalValueDouble * Math.Pow(10, 5));
        builder.Append(intValue);
        builder.Append('+');
        builder.Append(decimalValue);
        
        writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
    }
}