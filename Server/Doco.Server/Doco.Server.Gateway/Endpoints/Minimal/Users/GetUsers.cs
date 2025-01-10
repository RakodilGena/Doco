using Doco.Server.Gateway.Models.Responses.Users;
using Doco.Server.Gateway.Services.Users;

namespace Doco.Server.Gateway.Endpoints.Minimal.Users;

internal static partial class UserEndpoints
{
    private static RouteGroupBuilder MapGetUsers(this RouteGroupBuilder group)
    {
        group.MapGet("get", GetUsers);
        return group;
    }

    /// <summary>
    /// Allows to get all users.
    /// </summary>
    /// <param name="getUsersService"></param>
    /// <param name="ct"></param>
    private static Task<GetUsersResultDto> GetUsers(
        IGetUsersService getUsersService,
        CancellationToken ct)
        => getUsersService.GetUsersAsync(ct);
}