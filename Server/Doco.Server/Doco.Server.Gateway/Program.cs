using Doco.Server.Gateway.Authentication;
using Doco.Server.Gateway.Authentication.Extensions;
using Doco.Server.Gateway.Authentication.Handlers;
using Doco.Server.Gateway.Authentication.Options;
using Doco.Server.Gateway.Authentication.Services;
using Doco.Server.Gateway.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
    
    var jwtAuthConfig = builder.AddJwtAuthServices();

    builder.Services.AddAuthorization(o =>
    {
        o.AddPolicy(JwtAuthConfig.PolicyName, 
            policy => policy
                .RequireAuthenticatedUser()
                .AddRequirements(new DocoAuthRequirement(DocoClaimTypes.UserId)));
    });
    
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => options.ApplyValidationParameters(jwtAuthConfig));
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