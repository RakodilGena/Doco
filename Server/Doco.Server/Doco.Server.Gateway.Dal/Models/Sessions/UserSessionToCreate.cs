namespace Doco.Server.Gateway.Dal.Models.Sessions;

public sealed record UserSessionToCreate(
    Guid UserId,
    string JwtToken,
    string RefreshToken);