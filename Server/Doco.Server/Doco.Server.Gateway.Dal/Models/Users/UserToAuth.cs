namespace Doco.Server.Gateway.Dal.Models.Users;

public sealed class UserToAuth
{
    public Guid Id { get; init; }
    
    public byte[] HashedPassword { get; init; } = null!;
    public byte[] HashPasswordSalt { get; init; } = null!;
    
    public DateTime? DeletedAt { get; init; }
}