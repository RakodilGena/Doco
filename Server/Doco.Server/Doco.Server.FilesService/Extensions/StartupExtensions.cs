using Doco.Server.FilesService.Infrastructure.Interceptors;
using Doco.Server.FilesService.Options;
using Doco.Server.FilesService.Services.Grpc.Files;

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

    public static WebApplicationBuilder AddGrpc(this WebApplicationBuilder builder)
    {
        builder.Services.AddGrpc(options =>
        {
            options.Interceptors.Add<ServerExceptionInterceptor>();
        });
        
        return builder;
    }

    public static WebApplication MapGrpcServices(this WebApplication app)
    {
        app.MapGrpcService<FileServiceImpl>();
        
        return app;
    }
}