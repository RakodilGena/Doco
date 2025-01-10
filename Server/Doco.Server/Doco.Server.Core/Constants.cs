namespace Doco.Server.Core;

public static class Constants
{
    public const int GrpcStreamChunkSize = 64 * 1024;//64kb

    // ReSharper disable once InconsistentNaming
    public const int UserTokenTTLHours = 24;
}