namespace Doco.Server.Gateway.Models.Domain.Auth;

internal sealed record LoginUserRequestDto(
    string Email,
    string Password);