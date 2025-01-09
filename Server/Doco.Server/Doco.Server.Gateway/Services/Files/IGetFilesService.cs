using Doco.Server.Gateway.Models.Responses.Files;

namespace Doco.Server.Gateway.Services.Files;

internal interface IGetFilesService
{
    Task<GetFilesResultDto> GetFilesAsync(
        Guid? folderId, 
        CancellationToken ct);
}