namespace Doco.Server.Gateway.ExceptionHandlers.Models.Auth;

/// <summary>
/// 
/// </summary>
/// <param name="SubType"></param>
/// <param name="Message"></param>
public sealed record HandledAuthException(
    HandledAuthExceptionType SubType,
    string Message)
    : HandledGlobalException(HandledGlobalExceptionType.Auth, Message);