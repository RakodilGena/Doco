namespace Doco.Server.Gateway.Models.Dtos.Users;

internal sealed record CreateUserRequestDto(
    string Email,
    string Name,
    string Password);