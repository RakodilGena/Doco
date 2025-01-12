using System.Security.Cryptography;
using System.Text;
using Doco.Server.Core;

namespace Doco.Server.Gateway.Authentication.Services.Impl;

internal sealed class RefreshTokenCreator : IRefreshTokenCreator
{
    public (string refreshToken, DateTime expiresAt) CreateRefreshToken()
    {
        Span<byte> span = stackalloc byte[128];

        using (var rng = RandomNumberGenerator.Create())
        {
            // The array is now filled with cryptographically strong random bytes.
            rng.GetBytes(span);
        }

        var token = Encoding.Unicode.GetString(span);
        var expiresAt = DateTime.UtcNow.AddHours(Constants.RefreshTokenTTLHours);

        return (token, expiresAt);
    }
}