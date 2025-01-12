using Doco.Server.Gateway.Models.Dtos.Auth;
using Doco.Server.Gateway.Models.Requests.Auth;

namespace Doco.Server.Gateway.Mappers;

internal static class RefreshSessionRequestMapper
{
    public static RefreshSessionRequestDto ToDto(this RefreshSessionRequest request)
    {
        return new RefreshSessionRequestDto(
            request.UserId!.Value,
            request.RefreshToken!);
    }
}