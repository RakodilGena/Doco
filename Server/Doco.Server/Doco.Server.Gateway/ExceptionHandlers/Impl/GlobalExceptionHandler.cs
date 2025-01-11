using System.Net;
using Doco.Server.Gateway.Dal.Exceptions.Users;
using Doco.Server.Gateway.ExceptionHandlers.Models;
using Doco.Server.Gateway.Exceptions.Auth;
using ValidationException = FluentValidation.ValidationException;

namespace Doco.Server.Gateway.ExceptionHandlers.Impl;

internal sealed partial class GlobalExceptionHandler : IGlobalExceptionHandler
{
    public async Task HandleAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        await HandleEx(exception, context);
    }

    private static Task WriteAsJsonAsync<THandledEx>(THandledEx ex, HttpContext context)
        => context.Response.WriteAsJsonAsync(ex);

    private static Task HandleEx(
        Exception exception,
        HttpContext context)
    {
        return exception switch
        {
            ValidationException ex => HandleExValidation(ex, context),
            AuthExceptionBase ex => HandleExAuth(ex, context),
            DbUserExceptionBase ex => HandleExDbUser(ex, context),

            _ => HandleExGlobal(exception, context)
        };
    }

    private static Task HandleExGlobal(
        Exception exception,
        HttpContext context)
    {
        var ex = new HandledGlobalException(
            HandledGlobalExceptionType.Unknown,
            exception.Message);

        return WriteAsJsonAsync(ex, context);
    }

    private static partial Task HandleExValidation(
        ValidationException exception,
        HttpContext context);

    private static partial Task HandleExAuth(
        AuthExceptionBase exception,
        HttpContext context);

    private static partial Task HandleExDbUser(
        DbUserExceptionBase exception,
        HttpContext context);
}