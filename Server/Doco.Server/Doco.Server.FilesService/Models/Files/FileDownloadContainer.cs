namespace Doco.Server.FilesService.Models.Files;

internal sealed record FileDownloadContainer(
    Stream Stream,
    string FileName,
    string ContentType);