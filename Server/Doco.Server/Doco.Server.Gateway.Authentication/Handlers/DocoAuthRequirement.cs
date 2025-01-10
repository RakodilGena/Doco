using Microsoft.AspNetCore.Authorization;

namespace Doco.Server.Gateway.Authentication.Handlers;

public sealed class DocoAuthRequirement : IAuthorizationRequirement
{
    public DocoAuthRequirement(string claimType) => ClaimType = claimType;
    public string ClaimType { get; set; }
}