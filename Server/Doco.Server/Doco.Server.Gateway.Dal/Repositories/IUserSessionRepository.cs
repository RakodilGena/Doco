using Doco.Server.Gateway.Dal.Models.Sessions;

namespace Doco.Server.Gateway.Dal.Repositories;

public interface IUserSessionRepository
{
    Task AddUserSessionAsync(
        UserSessionToCreate userSession,
        CancellationToken ct);
}