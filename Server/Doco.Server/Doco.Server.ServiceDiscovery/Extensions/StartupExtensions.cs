using Doco.Server.ServiceDiscovery.Options;

namespace Doco.Server.ServiceDiscovery.Extensions;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
    {
        var fileServicesUrlSection = builder.Configuration.GetSection(FileServicesUrls.SectionName);
        if (fileServicesUrlSection.Exists() is false)
        {
            throw new Exception($"{FileServicesUrls.SectionName} section is not set");
        }
        
        builder.Services.Configure<FileServicesUrls>(fileServicesUrlSection);
        
        return builder;
    }
}