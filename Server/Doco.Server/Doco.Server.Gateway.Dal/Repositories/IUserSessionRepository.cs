using Doco.Server.Gateway.Dal.Models.Sessions;

namespace Doco.Server.Gateway.Dal.Repositories;

public interface IUserSessionRepository
{
    Task AddUserSessionAsync(
        UserSessionToCreate session,
        CancellationToken ct);
    
    Task<bool> UserSessionIsValidAsync(
        Guid userId,
        string jwtToken);
    
    Task<bool> UserCanRefreshSessionAsync(
        Guid userId,
        string refreshToken,
        CancellationToken ct);

    Task RefreshUserSessionAsync(
        UserSessionToRefresh session,
        CancellationToken ct);

    Task RemoveSessionAsync(
        Guid userId,
        string jwtToken);
}