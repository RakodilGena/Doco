using Doco.Server.Gateway.ExceptionHandlers.Models;
using FluentValidation;

namespace Doco.Server.Gateway.ExceptionHandlers.Impl;

internal partial class GlobalExceptionHandler
{
    private static partial Task HandleExValidation(
        ValidationException exception,
        HttpContext context)
    {
        var ex = new HandledGlobalException(
            HandledGlobalExceptionType.Validation,
            exception.Message);

        return WriteAsJsonAsync(ex, context);
    }
}