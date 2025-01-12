using Microsoft.AspNetCore.Http;

namespace Doco.Server.Gateway.Authentication.Services.Impl;

internal sealed class JwtTokenValuesFetcher : IJwtTokenValuesFetcher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtTokenValuesFetcher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public bool IsAuthorized()
    {
        var userIdClaim = GetClaimValue(DocoClaimTypes.UserId);
        return userIdClaim != null;
    }

    public Guid FetchUserId()
    {
        var userIdClaim = GetClaimValue(DocoClaimTypes.UserId);
        
        return Guid.Parse(userIdClaim!);
    }

    public string FetchRawToken()
    {
        string? tokenString = _httpContextAccessor.HttpContext!
            .Request.Headers.Authorization.First()!.Split(" ").Last();

        return tokenString;
    }
    
    private string? GetClaimValue(string claimType)
    {
        return _httpContextAccessor.HttpContext?.User
            .Claims.FirstOrDefault(claim => claim.Type == claimType)?.Value;
    }
}