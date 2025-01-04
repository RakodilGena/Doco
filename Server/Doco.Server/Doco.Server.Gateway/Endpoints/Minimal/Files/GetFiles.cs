using Doco.Server.Gateway.Services;

namespace Doco.Server.Gateway.Endpoints.Minimal.Files;

internal static partial class FileEndpoints
{
    private static IEndpointRouteBuilder MapGetFiles(this IEndpointRouteBuilder app)
    {
        app.MapGet($"{Route}/get", GetFiles);
        return app;
    }

    /// <summary>
    /// Allows user get all accessible files.
    /// </summary>
    /// <param name="getFilesService"></param>
    /// <param name="ct"></param>
    private static Task GetFiles(
        IGetFilesService getFilesService,
        CancellationToken ct)
        => getFilesService.GetFilesAsync(ct);
}