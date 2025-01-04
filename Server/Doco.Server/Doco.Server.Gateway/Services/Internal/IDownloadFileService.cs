using Doco.Server.Gateway.Models.Responses;

namespace Doco.Server.Gateway.Services.Internal;

internal interface IDownloadFileService
{
    Task<FileDownloadResultDto> DownloadFileAsync(
        Guid fileId, 
        CancellationToken ct);
}