using Doco.Server.Gateway.Exceptions.Auth;
using Doco.Server.Gateway.Models.Dtos.Auth;
using Doco.Server.Gateway.Models.Responses.Auth;

namespace Doco.Server.Gateway.Services.Auth;

internal interface IRefreshSessionService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="RefreshSessionException"></exception>
    Task<LoginUserResult> RefreshSessionAsync(
        RefreshSessionRequestDto request, 
        CancellationToken ct);
}