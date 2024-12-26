namespace Comdirect.NET.Extensions;

public static class CancellationTokenExtensions {
    public static bool IsCancelled(this CancellationToken? cancellationToken) {
        return cancellationToken is not null && cancellationToken.Value.IsCancellationRequested;
    }
}