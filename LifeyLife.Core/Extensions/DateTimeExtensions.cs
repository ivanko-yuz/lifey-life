namespace LifeyLife.Core.Extensions;

public static class DateTimeExtensions
{
    private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static long ToUnixUtcTimeStamp(this DateTime value)
    {
        var elapsedTime = value - Epoch;
        return (long)elapsedTime.TotalSeconds;
    }
    
    public static DateTime ToDateTime( double unixTimeStamp )
    {
        // Unix timestamp is seconds past epoch
        return Epoch.AddSeconds( unixTimeStamp ).ToLocalTime();
    }
}