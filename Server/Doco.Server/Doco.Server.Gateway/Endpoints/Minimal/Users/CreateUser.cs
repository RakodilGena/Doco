using Doco.Server.Gateway.Models.Requests;
using Doco.Server.Gateway.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Doco.Server.Gateway.Endpoints.Minimal.Users;

internal static partial class UserEndpoints
{
    private static IEndpointRouteBuilder MapCreateUser(this IEndpointRouteBuilder group)
    {
        group.MapPost("create/", CreateUser);
        return group;
    }

    /// <summary>
    /// Allows to create new user.
    /// </summary>
    /// <param name="createUserService"></param>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    private static Task CreateUser(
        ICreateUserService createUserService,
        [FromBody] CreateUserRequest request,
        CancellationToken ct)
        => createUserService.CreateUserAsync(request, ct);
}