namespace Doco.Server.Gateway.Exceptions.Auth;

internal sealed class InvalidLoginCredentialsException(string message)
    : AuthExceptionBase(message);