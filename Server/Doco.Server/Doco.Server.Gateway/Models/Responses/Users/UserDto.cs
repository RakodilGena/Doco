#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace Doco.Server.Gateway.Models.Responses.Users;

public sealed class UserDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
    public bool IsAdmin { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? DeletedAt { get; init; }
}