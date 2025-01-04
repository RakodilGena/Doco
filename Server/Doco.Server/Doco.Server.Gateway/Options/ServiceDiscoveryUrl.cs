namespace Doco.Server.Gateway.Options;

internal sealed class ServiceDiscoveryUrl
{
    public const string SectionName = "ServiceDiscovery";
    
    public string Value { get; set; } = null!;
}