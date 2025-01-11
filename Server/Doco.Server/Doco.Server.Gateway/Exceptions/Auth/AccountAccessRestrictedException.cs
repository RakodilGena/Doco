namespace Doco.Server.Gateway.Exceptions.Auth;

internal sealed class AccountAccessRestrictedException(string message)
    : AuthExceptionBase(message);