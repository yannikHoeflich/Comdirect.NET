using System.Text.Json;
using System.Text.Json.Serialization;
using Comdirect.NET.Converters.Json;

namespace Comdirect.NET.Models;

public record RequestInfo(
    [property: JsonPropertyName("clientRequestId")]
    RequestId ClientRequestId
) {
    public string ToJson() {
        return JsonSerializer.Serialize(this);
    }
}

public record struct SessionId(string Id) {
    public static SessionId Generate() {
        string id = Guid.CreateVersion7().ToString("N");
        if (id.Length > 32) { // Shouldn't happen, just to be save
            id = id[..32];
        }

        return new SessionId(id);
    }
}

public record struct RequestId(
    [property: JsonPropertyName("sessionId")]
    string SessionId,
    [property: JsonPropertyName("requestId"), JsonConverter(typeof(StringToInt))]
    int Id) {
    private static readonly Lock CounterLock = new();
    private static uint s_counter = 0;
    
    public static RequestId Generate(SessionId sessionId) {
        DateTime now = DateTime.UtcNow;
        int id = now.Hour;
        id *= 100;
        id += now.Minute;
        id *= 100;
        id += now.Second;
        id *= 1000;
        using (CounterLock.EnterScope()) {
            id += (int)(s_counter % 1000);
            s_counter++;
        }

        return new RequestId(sessionId.Id, id);
    }
}