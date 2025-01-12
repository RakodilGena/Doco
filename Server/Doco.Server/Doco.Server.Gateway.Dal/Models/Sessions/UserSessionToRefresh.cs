namespace Doco.Server.Gateway.Dal.Models.Sessions;

public sealed record UserSessionToRefresh(
    Guid UserId,
    string OldRefreshToken,

    string NewJwtToken,
    string NewRefreshToken,
    DateTime NewRefreshTokenExpiresAt);