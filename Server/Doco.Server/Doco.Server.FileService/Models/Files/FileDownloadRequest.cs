namespace Doco.Server.FileService.Models.Files;

internal sealed record FileDownloadRequest(
    Guid UserId,
    Guid FileId);