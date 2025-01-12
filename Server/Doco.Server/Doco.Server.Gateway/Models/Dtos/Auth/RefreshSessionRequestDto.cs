namespace Doco.Server.Gateway.Models.Dtos.Auth;

internal sealed record RefreshSessionRequestDto(
    Guid UserId,
    string RefreshToken);