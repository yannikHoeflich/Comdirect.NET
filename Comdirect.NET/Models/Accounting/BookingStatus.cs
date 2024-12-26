using System.Text.Json.Serialization;

namespace Comdirect.NET.Models.Accounting;

public enum BookingStatus {
    [JsonStringEnumMemberName("BOOKED")]
    Booked,
    [JsonStringEnumMemberName("NOTBOOKED")]
    NotBooked
}