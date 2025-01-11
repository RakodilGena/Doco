namespace Doco.Server.Gateway.ExceptionHandlers;

internal interface IGlobalExceptionHandler
{
    Task HandleAsync(HttpContext context, Exception exception);
}