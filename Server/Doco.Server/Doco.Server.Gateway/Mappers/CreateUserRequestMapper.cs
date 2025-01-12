using Doco.Server.Gateway.Models.Dtos.Users;
using Doco.Server.Gateway.Models.Requests.Users;

namespace Doco.Server.Gateway.Mappers;

internal static class CreateUserRequestMapper
{
    public static CreateUserRequestDto ToDto(this CreateUserRequest request)
        => new(
            request.Email!.Trim(),
            request.Name!.Trim(),
            request.Password!.Trim());
}