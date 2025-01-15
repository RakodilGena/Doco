using Doco.Server.FileService.Dal.Config;
using Doco.Server.FileService.Infrastructure.Interceptors;
using Doco.Server.FileService.Options;
using Doco.Server.FileService.Services.Grpc.Files;

namespace Doco.Server.FileService.Extensions;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
    {
        #region fileStoreLimit
        
        var fileServicesUrlSection = builder.Configuration.GetSection(FileStoreDbLimit.SectionName);
        if (fileServicesUrlSection.Exists() is false)
        {
            throw new Exception($"{FileStoreDbLimit.SectionName} section is not set");
        }
        
        builder.Services.Configure<FileStoreDbLimit>(fileServicesUrlSection);

        #endregion

        #region db connection config

        {
            var connectionConfigSection = builder.Configuration.GetSection(PostgreSqlConnectionConfig.SectionName);
            if (connectionConfigSection.Exists() is false)
            {
                throw new Exception($"{PostgreSqlConnectionConfig.SectionName} section is not set");
            }

            builder.Services.Configure<PostgreSqlConnectionConfig>(connectionConfigSection);
        }

        #endregion
        
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
    
    public static bool InMigratorMode(this WebApplicationBuilder builder)
    {
        var needMigration = builder.Configuration.GetValue<string>("MIGRATE") is "true";
        return needMigration;
    }
}