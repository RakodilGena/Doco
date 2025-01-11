namespace Doco.Server.Gateway.ExceptionHandlers.Models.Users;

/// <summary>
/// 
/// </summary>
/// <param name="SubType"></param>
/// <param name="Message"></param>
public sealed record HandledUserException(
    HandledUserExceptionType SubType,
    string Message)
    : HandledGlobalException(HandledGlobalExceptionType.User, Message);