using Doco.Server.Gateway.Models.Requests.Users;

namespace Doco.Server.Gateway.Services.Users;

internal interface ICreateUserService
{
    Task CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken);
}