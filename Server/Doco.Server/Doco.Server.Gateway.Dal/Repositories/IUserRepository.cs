using Doco.Server.Gateway.Dal.Exceptions.Users;
using Doco.Server.Gateway.Dal.Models.Users;

namespace Doco.Server.Gateway.Dal.Repositories;

public interface IUserRepository
{
    Task<UserToAuth?> GetAuthUserAsync(string email, CancellationToken cancellationToken);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="DbUserEmailNotUniqueException"></exception>
    /// <exception cref="DbUserNameNotUniqueException"></exception>
    /// <returns></returns>
    Task CreateUserAsync(UserToCreate user, CancellationToken cancellationToken);
    
    Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken cancellationToken);
}