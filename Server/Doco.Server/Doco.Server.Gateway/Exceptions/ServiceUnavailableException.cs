namespace Doco.Server.Gateway.Exceptions;

internal sealed class ServiceUnavailableException(string message, Exception innerException)
    : Exception(message, innerException);