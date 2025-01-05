using Doco.Server.FilesService.Options;

namespace Doco.Server.FilesService.Extensions;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
    {
        var fileServicesUrlSection = builder.Configuration.GetSection(FileStoreDbLimit.SectionName);
        if (fileServicesUrlSection.Exists() is false)
        {
            throw new Exception($"{FileStoreDbLimit.SectionName} section is not set");
        }
        
        builder.Services.Configure<FileStoreDbLimit>(fileServicesUrlSection);
        
        return builder;
    }
}