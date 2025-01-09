namespace Doco.Server.Gateway.Models.Responses.Files;

/// <summary>
/// 
/// </summary>
public sealed record GetFilesResultDto(
    IReadOnlyList<FileDto> Files,
    IReadOnlyList<FolderDto> Folders)
{
    internal static GetFilesResultDto Empty => new([], []);
}