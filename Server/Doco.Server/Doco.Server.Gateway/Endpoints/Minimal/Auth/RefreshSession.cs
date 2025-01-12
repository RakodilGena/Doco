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
    private static RouteGroupBuilder MapRefreshSession(this RouteGroupBuilder group)
    {
        group.MapPost("/refreshSession", RefreshSession);
        return group;
    }

    /// <summary>
    /// Allows user to refresh session.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="refreshSessionService"></param>
    /// <param name="ct"></param>
    [AllowAnonymous]
    private static async Task<LoginUserResult> RefreshSession(
        [FromBody] RefreshSessionRequest? request,
        IRefreshSessionService refreshSessionService,
        CancellationToken ct)
    {
        var validator = new RefreshSessionRequestValidator();
        await validator.ValidateAndThrowAsync(request, ct);
        var requestDto = request!.ToDto();

        return await refreshSessionService.RefreshSessionAsync(requestDto, ct);
    }
}