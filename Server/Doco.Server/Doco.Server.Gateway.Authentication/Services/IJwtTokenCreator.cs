namespace Doco.Server.Gateway.Authentication.Services;

// ReSharper disable once InconsistentNaming
public interface IJwtTokenCreator
{
    public string CreateToken(Guid userId);
}