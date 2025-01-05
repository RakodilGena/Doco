namespace Doco.Server.Gateway.Services.Internal;

internal interface IUploadFileService
{
    Task UploadFilesAsync(
        Guid? folderId,
        IFormFileCollection files, 
        CancellationToken ct);
}