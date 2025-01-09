namespace Doco.Server.Gateway.Models.Responses.Files;

/// <summary>
/// 
/// </summary>
public sealed record FileDto(
    Guid Id, 
    string Name,
    int SizeBytes);