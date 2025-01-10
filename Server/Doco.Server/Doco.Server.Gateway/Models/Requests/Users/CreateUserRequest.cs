namespace Doco.Server.Gateway.Models.Requests.Users;

/// <summary>
/// 
/// </summary>
/// <param name="Email"></param>
/// <param name="Name"></param>
/// <param name="Password"></param>
public sealed record CreateUserRequest(
    string? Email,
    string? Name,
    string? Password);