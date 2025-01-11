using Doco.Server.Gateway.Dal.Exceptions.Users;
using Doco.Server.Gateway.ExceptionHandlers.Models.Users;

namespace Doco.Server.Gateway.ExceptionHandlers.Impl;

internal partial class GlobalExceptionHandler
{
    private static partial Task HandleExDbUser(
        DbUserExceptionBase exception,
        HttpContext context)
    {
        HandledUserExceptionType subType = exception switch
        {
            DbUserEmailNotUniqueException => HandledUserExceptionType.EmailNotUnique,
            DbUserNameNotUniqueException => HandledUserExceptionType.NameNotUnique,

            _ => HandledUserExceptionType.Unknown
        };

        var ex = new HandledUserException(subType, exception.Message);

        return WriteAsJsonAsync(ex, context);
    }
}