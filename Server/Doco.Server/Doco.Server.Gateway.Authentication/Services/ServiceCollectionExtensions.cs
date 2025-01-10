using Doco.Server.Gateway.Authentication.Handlers;
using Doco.Server.Gateway.Authentication.Options;
using Doco.Server.Gateway.Authentication.Services.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Doco.Server.Gateway.Authentication.Services;

public static class ServiceCollectionExtensions
{
    public static JwtAuthConfig AddJwtAuthServices(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<JwtAuthConfig>()
            .BindConfiguration(JwtAuthConfig.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        builder.Services
            .AddScoped<IJWTokenCreator, JWTokenCreator>()
            .AddSingleton<IAuthorizationHandler, DocoAuthRequirementHandler>();
        
        var jwtSection = builder.Configuration.GetSection(JwtAuthConfig.SectionName);
        var jwtAuthConfig = jwtSection.Get<JwtAuthConfig>();
        return jwtAuthConfig!;
    }
}