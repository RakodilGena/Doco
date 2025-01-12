namespace Doco.Server.Gateway.Exceptions.Auth;

internal sealed class RefreshSessionException(string message)
    : AuthExceptionBase(message);