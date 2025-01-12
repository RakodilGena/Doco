// ReSharper disable InconsistentNaming
namespace Doco.Server.Core;

public static class Constants
{
    public const int GrpcStreamChunkSize = 64 * 1024;//64kb
    
    public const int JwtTokenTTLHours = 24;
    public const int RefreshTokenTTLHours = 24*7;
}