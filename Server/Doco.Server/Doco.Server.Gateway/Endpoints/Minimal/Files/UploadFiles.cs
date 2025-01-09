using Doco.Server.Gateway.Services.Files;
using Microsoft.AspNetCore.Mvc;

namespace Doco.Server.Gateway.Endpoints.Minimal.Files;

internal static partial class FileEndpoints
{
    private static IEndpointRouteBuilder MapUploadFiles(this IEndpointRouteBuilder app)
    {
        app.MapPost($"{Route}/upload", UploadFiles);
        return app;
    }

    /// <summary>
    /// Allows user to upload one or more files to specific folder.
    /// </summary>
    /// <param name="folderId"></param>
    /// <param name="files"></param>
    /// <param name="uploadFileService"></param>
    /// <param name="ct"></param>
    private static Task UploadFiles(
        [FromForm] Guid? folderId,
        [FromForm] IFormFileCollection files,
        IUploadFileService uploadFileService,
        CancellationToken ct)
        => uploadFileService.UploadFilesAsync(folderId, files, ct);
}