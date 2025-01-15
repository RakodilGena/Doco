using Doco.Server.FileService.Extensions;
using Doco.Server.FileService.Services.Internal;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

var inStandardMode = builder.InMigratorMode() is false;

if (inStandardMode)
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ConfigureEndpointDefaults(x =>
            x.Protocols = HttpProtocols.Http2);
    });

    builder
        .AddOptions()
        .AddServices()
        .AddGrpc();
}

var app = builder.Build();

if (inStandardMode)
{
    app.MapGrpcServices();

    app.MapGet("/",
        () =>
            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
}

RunWithMigrate();
return;


void RunWithMigrate()
{
    if (inStandardMode)
    {
        app.Run();
    }
    else
    {
        Migrator.RunMigrations(builder);
    }
}