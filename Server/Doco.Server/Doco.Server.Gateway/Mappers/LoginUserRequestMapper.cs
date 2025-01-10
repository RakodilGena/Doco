using Doco.Server.Gateway.Models.Domain.Auth;
using Doco.Server.Gateway.Models.Requests.Auth;

namespace Doco.Server.Gateway.Mappers;

internal static class LoginUserRequestMapper
{
    public static LoginUserRequestDto ToDto(this LoginUserRequest request)
        => new(
            request.Email!.Trim(),
            request.Password!.Trim());
}