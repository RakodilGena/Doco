using Doco.Server.Gateway.Models.Requests.Users;
using Doco.Server.Gateway.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Doco.Server.Gateway.Endpoints.Minimal.Users;

internal static partial class UserEndpoints
{
    private static RouteGroupBuilder MapCreateUser(this RouteGroupBuilder group)
    {
        group.MapPost("create", CreateUser);
        return group;
    }

    /// <summary>
    /// Allows to create new user.
    /// </summary>
    /// <param name="createUserService"></param>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    //todo: create rate limiter
    [AllowAnonymous]
    private static Task CreateUser(
        ICreateUserService createUserService,
        [FromBody] CreateUserRequest request,
        CancellationToken ct)
        => createUserService.CreateUserAsync(request, ct);
}