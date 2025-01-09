namespace Doco.Server.Gateway.Services.Files;

internal interface IUploadFileService
{
    Task UploadFilesAsync(
        Guid? folderId,
        IFormFileCollection files, 
        CancellationToken ct);
}