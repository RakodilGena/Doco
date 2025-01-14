using Doco.Server.FilesService.Services.Internal.UserData;

namespace Doco.Server.FilesService.Services.Internal;

internal static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserIdFetcher, UserIdFetcher>();
        
        return builder;
    }
}