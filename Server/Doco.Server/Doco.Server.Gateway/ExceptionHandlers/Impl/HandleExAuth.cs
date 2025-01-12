using Doco.Server.Gateway.ExceptionHandlers.Models.Auth;
using Doco.Server.Gateway.Exceptions.Auth;

namespace Doco.Server.Gateway.ExceptionHandlers.Impl;

internal partial class GlobalExceptionHandler
{
    private static partial Task HandleExAuth(
        AuthExceptionBase exception,
        HttpContext context)
    {
        HandledAuthExceptionType subType = exception switch
        {
            AccountAccessRestrictedException => HandledAuthExceptionType.AccessRestricted,
            InvalidLoginCredentialsException => HandledAuthExceptionType.InvalidCredentials,
            RefreshSessionException => HandledAuthExceptionType.UnableToRefreshSession,
            
            _ => HandledAuthExceptionType.Unknown
        };

        var ex = new HandledAuthException(subType, exception.Message);

        return WriteAsJsonAsync(ex, context);
    }
}