using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Doco.Server.Core;
using Doco.Server.Gateway.Authentication.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Doco.Server.Gateway.Authentication.Services.Impl;

// ReSharper disable once InconsistentNaming
internal sealed class JwtTokenCreator : IJwtTokenCreator
{
    private readonly JwtAuthConfig _jwtConfig;

    public JwtTokenCreator(IOptions<JwtAuthConfig> jwtOptions)
    {
        _jwtConfig = jwtOptions.Value;
    }

    public string CreateToken(Guid userId)
    {
        DateTime expirationUtc = DateTime.UtcNow.AddHours(Constants.UserTokenTTLHours);

        var token = CreateJwtToken(userId, expirationUtc);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private JwtSecurityToken CreateJwtToken(
        Guid userId,
        DateTime expirationUtc)
    {
        var claims = CreateClaims(userId, expirationUtc);
        var credentials = CreateSigningCredentials();

        return new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: claims,
            expires: expirationUtc,
            signingCredentials: credentials
        );
    }

    private IEnumerable<Claim> CreateClaims(Guid userId, DateTime expirationUtc)
    {
        yield return new Claim(DocoClaimTypes.UserId, userId.ToString());

        yield return new Claim(ClaimTypes.Expiration, expirationUtc.ToString("O"));

        yield return new Claim(JwtRegisteredClaimNames.Iat,
            EpochTime.GetIntDate(DateTime.UtcNow).ToString(CultureInfo.InvariantCulture),
            ClaimValueTypes.Integer64);

        yield return new Claim(JwtRegisteredClaimNames.Sub, _jwtConfig.Sub);

        yield return new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
    }

    private SigningCredentials CreateSigningCredentials()
    {
        var sKey = _jwtConfig.GetSymmetricSecurityKey();
        return new SigningCredentials(
            sKey,
            SecurityAlgorithms.HmacSha256
        );
    }
}