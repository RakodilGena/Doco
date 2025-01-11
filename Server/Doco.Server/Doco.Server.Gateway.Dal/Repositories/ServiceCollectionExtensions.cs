using Doco.Server.Gateway.Dal.Repositories.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace Doco.Server.Gateway.Dal.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IUserSessionRepository, UserSessionRepository>();
    }
}