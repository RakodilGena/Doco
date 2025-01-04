namespace Doco.Server.Gateway.Options;

internal sealed class ServiceDiscoveryTimeout
{
    public const string SectionName = "ServiceDiscoveryTimeout";
    
    public int Seconds { get; set; }
}