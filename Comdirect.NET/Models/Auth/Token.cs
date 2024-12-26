namespace Comdirect.NET.Models.Auth;

public record struct Token(string AccessToken, string RefreshToken, long ExpiresAtTick) {
    public bool IsExpired => Environment.TickCount64 > ExpiresAtTick;
}