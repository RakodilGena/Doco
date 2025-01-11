namespace Doco.Server.Gateway.Dal.Exceptions.Users;

/// <summary>
/// 
/// </summary>
/// <param name="message"></param>
public sealed class DbUserEmailNotUniqueException(string message)
    : DbUserExceptionBase(message);