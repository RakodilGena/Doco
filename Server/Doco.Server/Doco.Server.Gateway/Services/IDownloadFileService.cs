using Doco.Server.Gateway.Models.Responses;

namespace Doco.Server.Gateway.Services;

internal interface IDownloadFileService
{
    Task<FileDownloadResultDto> DownloadFileAsync(
        Guid fileId, 
        CancellationToken ct);
}