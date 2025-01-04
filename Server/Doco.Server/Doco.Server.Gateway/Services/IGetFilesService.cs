using Doco.Server.Gateway.Models.Responses;

namespace Doco.Server.Gateway.Services;

internal interface IGetFilesService
{
    Task<GetFilesResultDto> GetFilesAsync(CancellationToken ct);
}