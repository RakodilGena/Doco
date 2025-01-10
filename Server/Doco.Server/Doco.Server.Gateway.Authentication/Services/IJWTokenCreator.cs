namespace Doco.Server.Gateway.Authentication.Services;

// ReSharper disable once InconsistentNaming
public interface IJWTokenCreator
{
    public string CreateToken(Guid userId);
}