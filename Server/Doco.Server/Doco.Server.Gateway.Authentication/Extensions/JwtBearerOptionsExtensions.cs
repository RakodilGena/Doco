using Doco.Server.Gateway.Authentication.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Doco.Server.Gateway.Authentication.Extensions;

internal static class JwtBearerOptionsExtensions
{
    public static void ApplyValidationParameters(
        this JwtBearerOptions options,
        JwtAuthConfig jwtAuthConfig)
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtAuthConfig.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtAuthConfig.Audience,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = jwtAuthConfig.GetSymmetricSecurityKey()
        };
    }
}