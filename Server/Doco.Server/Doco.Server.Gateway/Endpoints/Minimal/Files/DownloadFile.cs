using Doco.Server.Gateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace Doco.Server.Gateway.Endpoints.Minimal.Files;

internal static partial class FileEndpoints
{
    private static IEndpointRouteBuilder MapDownloadFile(this IEndpointRouteBuilder app)
    {
        app.MapGet($"{Route}/download/{{fileId:guid}}", DownloadFile);
        return app;
    }

    /// <summary>
    /// Allows the user to download certain file.
    /// </summary>
    /// <param name="fileId"></param>
    /// <param name="downloadFileService"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static async Task<FileResult> DownloadFile(
        Guid fileId, 
        IDownloadFileService downloadFileService,
        CancellationToken ct)
    {
        var result = await downloadFileService.DownloadFileAsync(fileId, ct);
        
        FileStreamResult fileStreamResult = new (result.Stream, result.ContentType)
        {
            FileDownloadName = result.FileDownloadName
        };
        
        return fileStreamResult;
    }
}