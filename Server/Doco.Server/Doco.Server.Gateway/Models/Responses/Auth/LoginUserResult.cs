namespace Doco.Server.Gateway.Models.Responses.Auth;

/// <summary>
/// 
/// </summary>
/// <param name="AuthToken"></param>
public sealed record LoginUserResult(
    string AuthToken);