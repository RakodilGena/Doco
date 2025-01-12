using System.Transactions;
using Doco.Server.Core.Extensions;
using Doco.Server.Gateway.Dal.Models.Users;
using Doco.Server.Gateway.Dal.Repositories;
using Doco.Server.Gateway.Models.Dtos.Users;
using Doco.Server.Gateway.Services.Transactions;
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
        using var tScope = TransactionScopeBuilder.Build(
            IsolationLevel.Serializable,
            timeout: 1.Seconds());

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