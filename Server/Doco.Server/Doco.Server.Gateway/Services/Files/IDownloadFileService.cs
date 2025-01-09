using Doco.Server.Gateway.Models.Responses.Files;

namespace Doco.Server.Gateway.Services.Files;

internal interface IDownloadFileService
{
    Task<FileDownloadResultDto> DownloadFileAsync(
        Guid fileId, 
        CancellationToken ct);
}