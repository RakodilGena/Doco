namespace Doco.Server.Gateway.Exceptions.Users;

/// <summary>
/// 
/// </summary>
/// <param name="message"></param>
public sealed class UserEmailNotUniqueException(string message)
    : UserExceptionBase(message);