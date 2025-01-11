namespace Doco.Server.Gateway.Dal.Exceptions.Users;

/// <summary>
/// 
/// </summary>
/// <param name="message"></param>
public abstract class DbUserExceptionBase(string message) : Exception(message);