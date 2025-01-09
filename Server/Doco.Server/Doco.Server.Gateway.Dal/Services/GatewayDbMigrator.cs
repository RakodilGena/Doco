using Doco.Server.Gateway.Dal.Config;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Doco.Server.Gateway.Dal.Services;

public static class GatewayDbMigrator
{
    public static void Migrate(PostgreSqlConnectionConfig connectionConfig)
    {
        var connectionString = BuildConnectionString(connectionConfig);

        using var serviceProvider = CreateServices(connectionString);
        using var scope = serviceProvider.CreateScope();
        // Put the database update into a scope to ensure
        // that all resources will be disposed.
        UpdateDatabase(scope.ServiceProvider);
    }

    private static string BuildConnectionString(PostgreSqlConnectionConfig connectionConfig)
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = connectionConfig.Host,
            Port = connectionConfig.Port,
            Database = connectionConfig.Database,
            Username = connectionConfig.Username,
            Password = connectionConfig.Password,
            CommandTimeout = connectionConfig.CommandTimeout
        };

        return connectionStringBuilder.ToString();
    }

    /// <summary>
    /// Configure the dependency injection services
    /// </summary>
    private static ServiceProvider CreateServices(string connectionString)
    {
        return new ServiceCollection()
            // Add common FluentMigrator services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb

                .AddPostgres()
                // Set the connection string
                .WithGlobalConnectionString(connectionString)
                // Define the assembly containing the migrations
                .ScanIn(typeof(GatewayDbMigrator).Assembly).For.Migrations())
            // Enable logging to console in the FluentMigrator way
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            // Build the service provider
            .BuildServiceProvider(false);
    }

    /// <summary>
    /// Update the database
    /// </summary>
    private static void UpdateDatabase(IServiceProvider serviceProvider)
    {
        // Instantiate the runner
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        // Execute the migrations
        runner.MigrateUp();
    }
}