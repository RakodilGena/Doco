using Doco.Server.Gateway.Services.Files;
using Microsoft.AspNetCore.Mvc;

namespace Doco.Server.Gateway.Endpoints.Minimal.Files;

internal static partial class FileEndpoints
{
    private static RouteGroupBuilder MapDownloadFile(this RouteGroupBuilder group)
    {
        //Won't work without $ IDK why.
        // ReSharper disable once RedundantStringInterpolation
        group.MapGet($"download/{{fileId:guid}}", DownloadFile);
        return group;
    }

    /// <summary>
    /// Allows user to download certain file.
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