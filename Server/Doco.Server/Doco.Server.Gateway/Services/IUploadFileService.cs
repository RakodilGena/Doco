namespace Doco.Server.Gateway.Services;

internal interface IUploadFileService
{
    Task UploadFilesAsync(IFormFileCollection files, CancellationToken ct);
}