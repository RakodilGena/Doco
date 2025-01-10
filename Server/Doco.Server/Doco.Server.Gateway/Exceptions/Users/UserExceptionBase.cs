namespace Doco.Server.Gateway.Exceptions.Users;

/// <summary>
/// 
/// </summary>
/// <param name="message"></param>
public abstract class UserExceptionBase(string message) : Exception(message);