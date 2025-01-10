using System.Transactions;
using Doco.Server.Core.Extensions;
using Doco.Server.Gateway.Models.Domain.Users;
using Doco.Server.Gateway.Services.Repositories;
using Doco.Server.PasswordEncryption;

namespace Doco.Server.Gateway.Services.Users.Impl;

internal sealed class CreateUserService : ICreateUserService
{
    private readonly IUserRepository _userRepository;

    public CreateUserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task CreateUserAsync(
        CreateUserRequestDto request,
        CancellationToken cancellationToken)
    {
        var options = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.Serializable,
            Timeout = 1.Seconds()
        };

        using var tScope = new TransactionScope(
            TransactionScopeOption.Required,
            options,
            TransactionScopeAsyncFlowOption.Enabled);

        var (hashedPass, hashSalt) = PasswordEncryptor.Encrypt(request.Password);

        var userToCreate = new UserToCreate(
            Id: Guid.CreateVersion7(),
            request.Name,
            request.Email,
            HashedPassword: hashedPass,
            HashPasswordSalt: hashSalt,
            IsAdmin: false);

        await _userRepository.CreateUserAsync(userToCreate, cancellationToken);

        tScope.Complete();
    }
}