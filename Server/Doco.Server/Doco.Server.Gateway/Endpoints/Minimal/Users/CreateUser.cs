using Doco.Server.Gateway.Mappers;
using Doco.Server.Gateway.Models.Requests.Users;
using Doco.Server.Gateway.Services.Users;
using Doco.Server.Gateway.Validation;
using FluentValidation;
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
    private static async Task CreateUser(
        [FromBody] CreateUserRequest? request,
        ICreateUserService createUserService,
        CancellationToken ct)
    {
        var validator = new CreateUserRequestValidator();
        await validator.ValidateAndThrowAsync(request, ct);
        var requestDto = request!.ToDto();

        await createUserService.CreateUserAsync(requestDto, ct);
    }
}