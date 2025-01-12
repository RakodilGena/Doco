using Doco.Server.Gateway.Authentication.Services;
using Doco.Server.Gateway.Dal.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Doco.Server.Gateway.Authentication.Handlers;

internal sealed class DocoAuthRequirementHandler : AuthorizationHandler<DocoAuthRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DocoAuthRequirementHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        //use to inject something here
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        DocoAuthRequirement requirement)
    {
        var tokenValuesFetcher = GetJwtTokenValuesFetcherService();
        if (tokenValuesFetcher.IsAuthorized())
        {
            var userId = tokenValuesFetcher.FetchUserId();
            var token = tokenValuesFetcher.FetchRawToken();

            var success = await CheckUserSessionIsValidAsync(userId, token);
            if (success)
            {
                context.Succeed(requirement);
                return;
            }
        }

        //var requiredClaim = context.User.Claims.FirstOrDefault(c => c.Type == requirement.ClaimType);

        // _httpContextAccessor.HttpContext.Response.ContentType = "application/json";
        // await _httpContextAccessor.HttpContext.Response.WriteAsJsonAsync(new { StatusCode = StatusCodes.Status401Unauthorized, Message = "Unauthorized. Required admin role." });
        // await _httpContextAccessor.HttpContext.Response.CompleteAsync();
        context.Fail();
    }

    private IJwtTokenValuesFetcher GetJwtTokenValuesFetcherService()
    {
        return _httpContextAccessor.HttpContext!
            .RequestServices.GetRequiredService<IJwtTokenValuesFetcher>();
    }

    private async Task<bool> CheckUserSessionIsValidAsync(Guid userId, string jwtToken)
    {
        var repo = _httpContextAccessor.HttpContext!
            .RequestServices.GetRequiredService<IUserSessionRepository>();

        return await repo.UserSessionIsValidAsync(userId, jwtToken);
    }
}