using Doco.Server.Gateway.Models.Responses.Users;

namespace Doco.Server.Gateway.Services.Users;

internal interface IGetUsersService
{
    Task<GetUsersResultDto> GetUsersAsync(CancellationToken cancellationToken);
}