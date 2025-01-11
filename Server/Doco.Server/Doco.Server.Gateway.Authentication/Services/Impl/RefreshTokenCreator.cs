using System.Security.Cryptography;
using System.Text;

namespace Doco.Server.Gateway.Authentication.Services.Impl;

internal sealed class RefreshTokenCreator : IRefreshTokenCreator
{
    public string CreateRefreshToken()
    {
        Span<byte> span = stackalloc byte[128];

        using (var rng = RandomNumberGenerator.Create())
        {
            // The array is now filled with cryptographically strong random bytes.
            rng.GetBytes(span);
        }

        return Encoding.Unicode.GetString(span);
    }
}