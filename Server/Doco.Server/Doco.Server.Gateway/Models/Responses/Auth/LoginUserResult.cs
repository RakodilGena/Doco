namespace Doco.Server.Gateway.Models.Responses.Auth;

/// <summary>
/// 
/// </summary>
/// <param name="JwtToken"></param>
/// <param name="RefreshToken"></param>
public sealed record LoginUserResult(
    string JwtToken,
    string RefreshToken);