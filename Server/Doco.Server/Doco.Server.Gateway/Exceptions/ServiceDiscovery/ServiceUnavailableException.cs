namespace Doco.Server.Gateway.Exceptions.ServiceDiscovery;

internal sealed class ServiceUnavailableException(string message, Exception innerException)
    : Exception(message, innerException);