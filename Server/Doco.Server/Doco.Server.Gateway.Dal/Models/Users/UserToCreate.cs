namespace Doco.Server.Gateway.Dal.Models.Users;

public sealed record UserToCreate(
    Guid Id,
    string Name,
    string Email,
    byte[] HashedPassword,
    byte[] HashPasswordSalt,
    bool IsAdmin);