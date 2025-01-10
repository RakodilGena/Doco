using Doco.Server.Gateway.Mappers;
using Doco.Server.Gateway.Models.Requests.Auth;
using Doco.Server.Gateway.Models.Responses.Auth;
using Doco.Server.Gateway.Services.Auth;
using Doco.Server.Gateway.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Doco.Server.Gateway.Endpoints.Minimal.Auth;

internal static partial class AuthEndpoints
{
    private static RouteGroupBuilder MapLogin(this RouteGroupBuilder group)
    {
        group.MapPost("/login", Login);
        return group;
    }

    /// <summary>
    /// Allows user to authenticate and retrieve auth token.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="loginUserService"></param>
    /// <param name="ct"></param>
    [AllowAnonymous]
    private static async Task<LoginUserResult> Login(
        [FromBody] LoginUserRequest? request,
        ILoginUserService loginUserService,
        CancellationToken ct)
    {
        var validator = new LoginUserRequestValidator();
        await validator.ValidateAndThrowAsync(request, ct);
        var requestDto = request!.ToDto();

        return await loginUserService.LoginUserAsync(requestDto, ct);
    }
}