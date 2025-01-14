namespace Doco.Server.FilesService.Models.Files;

internal sealed record FileDownloadRequest(
    Guid UserId,
    Guid FileId);