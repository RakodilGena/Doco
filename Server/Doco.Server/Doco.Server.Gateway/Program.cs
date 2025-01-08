using Doco.Server.Gateway.Dal.Config;
using Doco.Server.Gateway.Dal.Extensions;
using Doco.Server.Gateway.Extensions;

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
    var needMigration = builder.InMigratorMode();
    if (needMigration)
    {
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
    else
    {
        app.Run();
    }
}