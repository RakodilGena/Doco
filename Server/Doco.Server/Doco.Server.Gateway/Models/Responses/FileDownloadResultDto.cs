namespace Doco.Server.Gateway.Models.Responses;

internal readonly record struct FileDownloadResultDto(
    Stream Stream,
    string FileDownloadName,
    string ContentType);