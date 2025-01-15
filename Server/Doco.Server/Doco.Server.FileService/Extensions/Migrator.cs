using System.Diagnostics;
using Doco.Server.FileService.Dal.Config;
using Doco.Server.FileService.Dal.Services.Core;

namespace Doco.Server.FileService.Extensions;

internal static class Migrator
{
    public static void RunMigrations(WebApplicationBuilder builder)
    {
        Debug.Assert(builder.InMigratorMode());
        
        var connectionConfigSection = builder.Configuration.GetSection(PostgreSqlConnectionConfig.SectionName);
        if (connectionConfigSection.Exists() is false)
        {
            throw new Exception($"{PostgreSqlConnectionConfig.SectionName} section is not set");
        }

        var connectionConfig = connectionConfigSection.Get<PostgreSqlConnectionConfig>();
        if (connectionConfig is null)
        {
            throw new Exception($"{PostgreSqlConnectionConfig.SectionName} section is invalid");
        }

        Console.WriteLine("Ensuring database created...");
        GatewayDbCreator.EnsureDatabaseCreated(connectionConfig);
        Console.WriteLine("Done.");
        
        Console.WriteLine("Starting migrations...");
        GatewayDbMigrator.Migrate(connectionConfig);
        Console.WriteLine("Done.");
    }
}