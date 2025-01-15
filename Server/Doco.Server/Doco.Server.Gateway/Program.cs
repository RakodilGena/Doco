using Doco.Server.Gateway.Authentication.Extensions;
using Doco.Server.Gateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

var inStandardMode = builder.InMigratorMode() is false;

if (inStandardMode)
{
    builder.ConfigureMaxRequestSize()
        .AddSwagger()
        .AddOptions()
        .AddServices()
        .AddGrpcServices()
        .AddDaemons()
        .AddGlobalExceptionHandler();

    builder.Services.AddEndpointsApiExplorer();

    builder.AddCustomJwtAuthentication();
}

var app = builder.Build();

if (inStandardMode)
{
    app.UseAuthentication();
    app.UseAuthorization();

// Configure the HTTP request pipeline.
    app.UseSwagger();

    app.UseHttpsRedirection();

    app.UseGlobalExceptionHandler();

    app.MapEndpoints();
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