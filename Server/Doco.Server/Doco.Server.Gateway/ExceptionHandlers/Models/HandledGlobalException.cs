namespace Doco.Server.Gateway.ExceptionHandlers.Models;

/// <summary>
/// 
/// </summary>
public record HandledGlobalException(
    HandledGlobalExceptionType GlobalType,
    string Message);