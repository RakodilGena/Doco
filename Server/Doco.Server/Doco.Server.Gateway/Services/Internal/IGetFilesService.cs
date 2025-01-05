using Doco.Server.Gateway.Models.Responses;

namespace Doco.Server.Gateway.Services.Internal;

internal interface IGetFilesService
{
    Task<GetFilesResultDto> GetFilesAsync(
        Guid? folderId, 
        CancellationToken ct);
}