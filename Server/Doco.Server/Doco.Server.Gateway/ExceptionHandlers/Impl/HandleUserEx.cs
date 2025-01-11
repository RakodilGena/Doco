using Doco.Server.Gateway.ExceptionHandlers.Models.Users;
using Doco.Server.Gateway.Exceptions.Users;

namespace Doco.Server.Gateway.ExceptionHandlers.Impl;

internal partial class GlobalExceptionHandler
{
    private static partial Task HandleExUser(
        UserExceptionBase exception,
        HttpContext context)
    {
        HandledUserExceptionType subType = exception switch
        {
            UserEmailNotUniqueException => HandledUserExceptionType.EmailNotUnique,
            UserNameNotUniqueException => HandledUserExceptionType.NameNotUnique,

            _ => HandledUserExceptionType.Unknown
        };

        var ex = new HandledUserException(subType, exception.Message);

        return WriteAsJsonAsync(ex, context);
    }
}