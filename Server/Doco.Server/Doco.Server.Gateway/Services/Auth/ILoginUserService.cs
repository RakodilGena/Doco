using Doco.Server.Gateway.Models.Requests.Auth;
using Doco.Server.Gateway.Models.Responses.Auth;

namespace Doco.Server.Gateway.Services.Auth;

internal interface ILoginUserService
{
    public Task<LoginUserResult> LoginUserAsync(
        LoginUserRequest request, 
        CancellationToken ct);
}