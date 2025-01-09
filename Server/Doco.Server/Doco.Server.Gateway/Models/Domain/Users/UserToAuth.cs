namespace Doco.Server.Gateway.Models.Domain.Users;

internal sealed class UserToAuth
{
    public Guid Id { get; init; }
    
    public string HashedPassword { get; init; } = null!;
    public string HashPasswordSalt { get; init; } = null!;
    
    public DateTime? DeletedAt { get; init; }
}