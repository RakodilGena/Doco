using Doco.Server.FileService.Dal.Services.DatabaseAccess;
using Doco.Server.FileService.Dal.Services.DatabaseAccess.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace Doco.Server.FileService.Dal.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDalServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IDbConnectionProvider, DbConnectionProvider>();
    }
}