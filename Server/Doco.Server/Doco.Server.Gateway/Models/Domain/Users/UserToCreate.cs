namespace Doco.Server.Gateway.Models.Domain.Users;

internal sealed record UserToCreate(
    Guid Id,
    string Name,
    string Email,
    string HashedPassword,
    string HashPasswordSalt,
    bool IsAdmin);