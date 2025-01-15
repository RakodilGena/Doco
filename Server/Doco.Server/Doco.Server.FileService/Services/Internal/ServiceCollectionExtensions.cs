using Doco.Server.FileService.Services.Internal.UserData;

namespace Doco.Server.FileService.Services.Internal;

internal static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserIdFetcher, UserIdFetcher>();
        
        return builder;
    }
}