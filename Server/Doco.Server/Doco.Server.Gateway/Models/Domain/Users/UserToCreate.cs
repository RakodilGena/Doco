namespace Doco.Server.Gateway.Models.Domain.Users;

internal sealed record UserToCreate(
    Guid Id,
    string Name,
    string Email,
    byte[] HashedPassword,
    byte[] HashPasswordSalt,
    bool IsAdmin);