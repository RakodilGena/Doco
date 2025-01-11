using Doco.Server.Gateway.Dal.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Doco.Server.Gateway.Authentication.Handlers;

public sealed class DocoAuthRequirementHandler : AuthorizationHandler<DocoAuthRequirement>
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
        var requiredClaim = context.User.Claims.FirstOrDefault(c => c.Type == requirement.ClaimType);
        if (requiredClaim is not null)
        {
            var userId = Guid.Parse(requiredClaim.Value);

            var token = GetRawJwtToken();

            var success = await CheckUserSessionIsValidAsync(userId, token);
            if (success)
            {
                context.Succeed(requirement);
                return;
            }

        }

        // _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        // _httpContextAccessor.HttpContext.Response.ContentType = "application/json";
        // await _httpContextAccessor.HttpContext.Response.WriteAsJsonAsync(new { StatusCode = StatusCodes.Status401Unauthorized, Message = "Unauthorized. Required admin role." });
        // await _httpContextAccessor.HttpContext.Response.CompleteAsync();
        context.Fail();
    }

    private string GetRawJwtToken()
    {
        string tokenString = _httpContextAccessor.HttpContext!
            .Request.Headers.Authorization.First()!.Split(" ").Last();

        return tokenString;
    }

    private async Task<bool> CheckUserSessionIsValidAsync(Guid userId, string jwtToken)
    {
        var repo = _httpContextAccessor.HttpContext!
            .RequestServices.GetRequiredService<IUserRepository>();

        return await repo.NotDeletedUserWithSessionExistsAsync(userId, jwtToken);
    }
}