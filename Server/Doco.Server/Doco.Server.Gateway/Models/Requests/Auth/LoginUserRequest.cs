namespace Doco.Server.Gateway.Models.Requests.Auth;

/// <summary>
/// 
/// </summary>
/// <param name="Email"></param>
/// <param name="Password"></param>
public sealed record LoginUserRequest(
    string? Email,
    string? Password);