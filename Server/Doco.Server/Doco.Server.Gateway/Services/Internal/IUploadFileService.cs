namespace Doco.Server.Gateway.Services.Internal;

internal interface IUploadFileService
{
    Task UploadFilesAsync(IFormFileCollection files, CancellationToken ct);
}