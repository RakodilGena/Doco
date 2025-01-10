using Doco.Server.Gateway.Models.Domain.Auth;
using Doco.Server.Gateway.Models.Responses.Auth;

namespace Doco.Server.Gateway.Services.Auth;

internal interface ILoginUserService
{
    public Task<LoginUserResult> LoginUserAsync(
        LoginUserRequestDto request, 
        CancellationToken ct);
}