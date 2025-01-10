namespace Doco.Server.Gateway.Exceptions.Users;

/// <summary>
/// 
/// </summary>
/// <param name="message"></param>
public sealed class UserNameNotUniqueException(string message)
    : UserExceptionBase(message);