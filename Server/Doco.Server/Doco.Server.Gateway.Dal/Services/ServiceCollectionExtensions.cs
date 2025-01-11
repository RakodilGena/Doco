using Doco.Server.Gateway.Dal.Services.DatabaseAccess;
using Doco.Server.Gateway.Dal.Services.DatabaseAccess.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace Doco.Server.Gateway.Dal.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDalServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IDbConnectionProvider, DbConnectionProvider>();
    }
}