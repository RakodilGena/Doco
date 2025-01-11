using Doco.Server.Gateway.Dal.Models.Users;

namespace Doco.Server.Gateway.Models.Responses.Users;

/// <summary>
/// 
/// </summary>
public sealed record GetUsersResultDto(IReadOnlyList<UserDto> Users);