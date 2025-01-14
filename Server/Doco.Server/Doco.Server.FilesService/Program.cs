using Doco.Server.FilesService.Extensions;
using Doco.Server.FilesService.Services.Internal;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureEndpointDefaults(x =>
        x.Protocols = HttpProtocols.Http2);
});

builder
    .AddOptions()
    .AddServices()
    .AddGrpc();


var app = builder.Build();
app.MapGrpcServices();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();