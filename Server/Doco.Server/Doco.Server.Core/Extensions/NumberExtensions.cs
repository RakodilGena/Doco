namespace Doco.Server.Core.Extensions;

public static class NumberExtensions
{
    public static TimeSpan Seconds(this int seconds) => TimeSpan.FromSeconds(seconds);
}