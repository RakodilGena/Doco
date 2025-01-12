namespace Doco.Server.Gateway.Models.Dtos.Auth;

internal sealed record LoginUserRequestDto(
    string Email,
    string Password);