using Doco.Server.Gateway.Authentication.Handlers;
using Doco.Server.Gateway.Authentication.Options;
using Doco.Server.Gateway.Authentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Doco.Server.Gateway.Authentication.Extensions;

public static class StartupExtensions
{
    public static WebApplicationBuilder AddCustomJwtAuthentication(this WebApplicationBuilder builder)
    {
        builder.AddJwtAuthServices();

        builder.Services.AddSingleton<IAuthorizationHandler, DocoAuthRequirementHandler>();
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(JwtAuthConfig.PolicyName,
                policy =>
                    policy
                        .RequireAuthenticatedUser()
                        .AddRequirements(new DocoAuthRequirement( /*DocoClaimTypes.UserId*/)));

        
        var jwtSection = builder.Configuration.GetSection(JwtAuthConfig.SectionName);
        var jwtAuthConfig = jwtSection.Get<JwtAuthConfig>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.ApplyValidationParameters(jwtAuthConfig!));

        return builder;
    }
}