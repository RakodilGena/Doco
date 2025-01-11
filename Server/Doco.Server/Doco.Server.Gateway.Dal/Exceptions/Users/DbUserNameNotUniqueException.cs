namespace Doco.Server.Gateway.Dal.Exceptions.Users;

/// <summary>
/// 
/// </summary>
/// <param name="message"></param>
public sealed class DbUserNameNotUniqueException(string message)
    : DbUserExceptionBase(message);