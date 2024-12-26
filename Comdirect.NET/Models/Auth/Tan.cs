namespace Comdirect.NET.Models.Auth;

public record Tan(string Id, TanType Type, string? Challenge) {
    internal string? Link { get; init; }
}