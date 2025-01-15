namespace Doco.Server.Gateway.Models.Requests.Auth;

/// <summary>
/// 
/// </summary>
/// <param name="UserId"></param>
/// <param name="RefreshToken"></param>
public sealed record RefreshSessionRequest(
    Guid? UserId,
    string? RefreshToken);