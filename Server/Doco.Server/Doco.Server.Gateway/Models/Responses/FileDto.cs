namespace Doco.Server.Gateway.Models.Responses;

/// <summary>
/// 
/// </summary>
public sealed record FileDto(
    Guid Id, 
    string Name,
    int SizeBytes);