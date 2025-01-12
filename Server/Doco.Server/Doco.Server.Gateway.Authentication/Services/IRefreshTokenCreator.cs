namespace Doco.Server.Gateway.Authentication.Services;

public interface IRefreshTokenCreator
{
    public (string refreshToken, DateTime expiresAt) CreateRefreshToken();
}