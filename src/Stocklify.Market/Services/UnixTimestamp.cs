namespace Stocklify.Market.Services;

public static class UnixTimestamp
{
    public static long UtcNowInNanoseconds => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 1_000_000 + (DateTimeOffset.UtcNow.Ticks % TimeSpan.TicksPerMillisecond) * 100;
}