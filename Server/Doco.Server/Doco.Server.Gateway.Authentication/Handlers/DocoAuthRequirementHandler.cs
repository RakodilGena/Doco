using Microsoft.AspNetCore.Authorization;

namespace Doco.Server.Gateway.Authentication.Handlers;

public sealed class DocoAuthRequirementHandler : AuthorizationHandler<DocoAuthRequirement>
{
    public DocoAuthRequirementHandler()
    {
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
            
            //todo: replace later with some valid calls
            await Task.Yield();
            context.Succeed(requirement);
        }
        else
        {

            // _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            // _httpContextAccessor.HttpContext.Response.ContentType = "application/json";
            // await _httpContextAccessor.HttpContext.Response.WriteAsJsonAsync(new { StatusCode = StatusCodes.Status401Unauthorized, Message = "Unauthorized. Required admin role." });
            // await _httpContextAccessor.HttpContext.Response.CompleteAsync();

            context.Fail();

        }
    }
}