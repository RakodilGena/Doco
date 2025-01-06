using Doco.Server.Gateway.Dal.Extensions;
using Doco.Server.Gateway.Extensions;
using Doco.Server.Gateway.Options;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureMaxRequestSize()
    .AddSwagger()
    .AddServices()
    .AddGrpcServices()
    .AddDaemons();

builder.Services.AddEndpointsApiExplorer();



var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();

app.UseHttpsRedirection();

app.MapEndpoints();

RunWithMigrate();
return;


void RunWithMigrate()
{
    var needMigration = Environment.GetEnvironmentVariable("Migrate") is "true";
    if (needMigration)
    {
        var connectionConfigSection = builder.Configuration.GetSection(PostgreSqlConnectionConfig.SectionName);
        if (connectionConfigSection.Exists() is false)
        {
            throw new Exception($"{ServiceDiscoveryTimeout.SectionName} section is not set");
        }

        var connectionConfig = connectionConfigSection.Get<PostgreSqlConnectionConfig>();
        if (connectionConfig is null)
        {
            throw new Exception($"{ServiceDiscoveryTimeout.SectionName} section is invalid");
        }

        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = connectionConfig.Host,
            Port = connectionConfig.Port,
            Database = connectionConfig.Database,
            Username = connectionConfig.Username,
            Password = connectionConfig.Password,
            CommandTimeout = connectionConfig.CommandTimeout
        };

        var connectionString = connectionStringBuilder.ToString();
        MigrationExtensions.Migrate(connectionString);
    }
    else
    {
        app.Run();
    }
}