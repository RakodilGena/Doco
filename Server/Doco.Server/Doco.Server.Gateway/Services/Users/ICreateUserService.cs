using Doco.Server.Gateway.Models.Dtos.Users;

namespace Doco.Server.Gateway.Services.Users;

internal interface ICreateUserService
{
    Task CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken);
}