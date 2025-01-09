namespace Doco.Server.Gateway.Models.Responses.Files;

internal sealed record FileDownloadResultDto(
    Stream Stream,
    string FileDownloadName,
    string ContentType);