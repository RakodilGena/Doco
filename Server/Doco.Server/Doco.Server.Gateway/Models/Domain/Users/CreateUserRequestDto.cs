namespace Doco.Server.Gateway.Models.Domain.Users;

internal sealed record CreateUserRequestDto(
    string Email,
    string Name,
    string Password);