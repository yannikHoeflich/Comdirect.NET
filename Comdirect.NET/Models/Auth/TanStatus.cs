using System.Text.Json.Serialization;

namespace Comdirect.NET.Models.Auth;

public record TanStatus(
    [property: JsonPropertyName("authenticationId")]
    string Id,
    [property: JsonPropertyName("status"), JsonConverter(typeof(JsonStringEnumConverter<AuthenticationStatus>))]
    AuthenticationStatus Status
    );


public enum AuthenticationStatus {
    Authenticated,
    Pending
}