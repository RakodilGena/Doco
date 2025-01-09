using Doco.Server.Gateway.Models.Domain.Users;
using Doco.Server.Gateway.Models.Responses.Users;

namespace Doco.Server.Gateway.Services.Repositories;

internal interface IUserRepository
{
    Task<UserToAuth?> GetAuthUserAsync(string email, CancellationToken cancellationToken);
    
    Task CreateUserAsync(UserToCreate user, CancellationToken cancellationToken);
    
    Task<bool> UsersExistAsync(CancellationToken cancellationToken);
    
    Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken cancellationToken);
}