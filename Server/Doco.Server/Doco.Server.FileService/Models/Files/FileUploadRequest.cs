namespace Doco.Server.FileService.Models.Files;

internal sealed record FileUploadRequest(
    Guid UserId,
    Guid? FolderId,
    MemoryStream Stream,
    string FileName,
    string ContentType);