using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Doco.Server.Gateway.Authentication.Options;

public sealed class JwtAuthConfig
{
    public const string SectionName = "JwtAuthConfig";
    
    public const string PolicyName = "JwtAuth";
    
    [Required, MinLength(1)]
    public required string Issuer { get; set; } = null!;
    
    [Required, MinLength(1)]
    public required string Audience { get; set; } = null!;
    
    [Required, MinLength(1)]
    public required string Sub { get; set; } = null!;
    
    [Required, MinLength(24)]
    public required string Key { get; set; } = null!;
    
    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}