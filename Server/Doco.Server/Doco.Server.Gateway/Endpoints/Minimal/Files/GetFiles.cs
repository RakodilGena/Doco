using Doco.Server.Gateway.Models.Responses;
using Doco.Server.Gateway.Services.Internal;

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
    private static Task<GetFilesResultDto> GetFiles(
        IGetFilesService getFilesService,
        CancellationToken ct)
        => getFilesService.GetFilesAsync(ct);
}