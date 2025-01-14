using Doco.Server.Gateway.Authentication.Options;
using Doco.Server.Gateway.Authentication.Services.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Doco.Server.Gateway.Authentication.Services;

internal static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddJwtAuthServices(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<JwtAuthConfig>()
            .BindConfiguration(JwtAuthConfig.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        builder.Services
            .AddScoped<IJwtTokenCreator, JwtTokenCreator>()
            .AddSingleton<IRefreshTokenCreator, RefreshTokenCreator>()
            .AddScoped<IJwtTokenValuesFetcher, JwtTokenValuesFetcher>();

        return builder;
    }
}