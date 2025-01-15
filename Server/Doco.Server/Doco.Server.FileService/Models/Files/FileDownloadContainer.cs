namespace Doco.Server.FileService.Models.Files;

internal sealed record FileDownloadContainer(
    Stream Stream,
    string FileName,
    string ContentType);