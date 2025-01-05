using Doco.Server.Gateway.Models.Responses;
using Doco.Server.Gateway.Services.Internal;

namespace Doco.Server.Gateway.Endpoints.Minimal.Files;

internal static partial class FileEndpoints
{
    private static IEndpointRouteBuilder MapGetAllFiles(this IEndpointRouteBuilder app)
    {
        app.MapGet($"{Route}/get/", GetAllFiles);
        return app;
    }

    private static IEndpointRouteBuilder MapGetFilesFromFolder(this IEndpointRouteBuilder app)
    {
        app.MapGet($"{Route}/get/{{folderId:guid}}", GetFilesFromFolder);
        return app;
    }

    /// <summary>
    /// Allows user get all accessible files.
    /// </summary>
    /// <param name="getFilesService"></param>
    /// <param name="ct"></param>
    private static Task<GetFilesResultDto> GetAllFiles(
        IGetFilesService getFilesService,
        CancellationToken ct)
        => getFilesService.GetFilesAsync(folderId: null, ct);

    /// <summary>
    /// Allows user get accessible files from folder.
    /// </summary>
    /// <param name="folderId"></param>
    /// <param name="getFilesService"></param>
    /// <param name="ct"></param>
    private static Task<GetFilesResultDto> GetFilesFromFolder(
        Guid folderId,
        IGetFilesService getFilesService,
        CancellationToken ct)
        => getFilesService.GetFilesAsync(folderId, ct);
}