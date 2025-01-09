namespace Doco.Server.Gateway.Models.Responses.Files;

/// <summary>
/// 
/// </summary>
public  sealed record FolderDto(
    Guid Id, 
    string Name,
    IReadOnlyList<FileDto> Files);