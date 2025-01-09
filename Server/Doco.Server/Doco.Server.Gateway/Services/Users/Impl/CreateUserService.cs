using System.Transactions;
using Doco.Server.Core.Extensions;
using Doco.Server.Gateway.Models.Domain.Users;
using Doco.Server.Gateway.Models.Requests;
using Doco.Server.Gateway.Services.Repositories;

namespace Doco.Server.Gateway.Services.Users.Impl;

internal sealed class CreateUserService : ICreateUserService
{
    private readonly IUserRepository _userRepository;

    public CreateUserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var options = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.Serializable,
            Timeout = 1.Seconds()
        };
        
        using var tScope =  new TransactionScope(
            TransactionScopeOption.Required,
            options, 
            TransactionScopeAsyncFlowOption.Enabled);
        
        bool usersExist = await _userRepository.UsersExistAsync(cancellationToken);

        var salt = "rwefdsfdfs";

        var userToCreate = new UserToCreate(
            Id: Guid.CreateVersion7(),
            request.Name,
            request.Email,
            HashedPassword: request.Password + salt,
            HashPasswordSalt: salt,
            IsAdmin: usersExist is false);
        
        await _userRepository.CreateUserAsync(userToCreate, cancellationToken);
        
        tScope.Complete();
    }
}