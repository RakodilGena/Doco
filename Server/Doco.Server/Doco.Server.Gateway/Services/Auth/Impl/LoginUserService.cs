using System.Diagnostics.CodeAnalysis;
using System.Transactions;
using Doco.Server.Core.Extensions;
using Doco.Server.Gateway.Authentication.Services;
using Doco.Server.Gateway.Dal.Models.Sessions;
using Doco.Server.Gateway.Dal.Models.Users;
using Doco.Server.Gateway.Dal.Repositories;
using Doco.Server.Gateway.Exceptions.Auth;
using Doco.Server.Gateway.Models.Dtos.Auth;
using Doco.Server.Gateway.Models.Responses.Auth;
using Doco.Server.Gateway.Services.Transactions;
using Doco.Server.PasswordEncryption;

namespace Doco.Server.Gateway.Services.Auth.Impl;

internal sealed class LoginUserService : ILoginUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserSessionRepository _userSessionRepository;
    private readonly IJwtTokenCreator _jwtTokenCreator;
    private readonly IRefreshTokenCreator _refreshTokenCreator;

    public LoginUserService(
        IUserRepository userRepository,
        IJwtTokenCreator jwtTokenCreator,
        IRefreshTokenCreator refreshTokenCreator,
        IUserSessionRepository userSessionRepository)
    {
        _userRepository = userRepository;
        _jwtTokenCreator = jwtTokenCreator;
        _refreshTokenCreator = refreshTokenCreator;
        _userSessionRepository = userSessionRepository;
    }

    public async Task<LoginUserResult> LoginUserAsync(
        LoginUserRequestDto request,
        CancellationToken ct)
    {
        using var tScope = TransactionScopeBuilder.Build(
            IsolationLevel.ReadCommitted,
            timeout: 1.Seconds());

        var user = await FetchUserAsync(request, ct);

        EnsureUserIsValid(request, user);

        var (jwtToken, refreshToken) = await CreateNewSessionAsync(user, ct);

        tScope.Complete();

        return new LoginUserResult(jwtToken, refreshToken);
    }

    private async Task<UserToAuth> FetchUserAsync(
        LoginUserRequestDto request,
        CancellationToken ct)
    {
        var user = await _userRepository.GetAuthUserAsync(request.Email, ct);

        if (user == null)
            ThrowInvalidCredentials();

        return user;
    }

    private static void EnsureUserIsValid(
        LoginUserRequestDto request,
        UserToAuth user)
    {
        var passwordValid = PasswordEncryptor.ComparePasswords(
            request.Password,
            user.HashedPassword,
            user.HashPasswordSalt);

        if (passwordValid is false)
            ThrowInvalidCredentials();

        if (user.DeletedAt.HasValue)
            ThrowAccessRestricted();
    }

    private async Task<(string jwtToken, string refreshToken)> CreateNewSessionAsync(
        UserToAuth user,
        CancellationToken ct)
    {
        var jwtToken = _jwtTokenCreator.CreateToken(user.Id);
        var (refreshToken, expiresAt) = _refreshTokenCreator.CreateRefreshToken();

        var session = new UserSessionToCreate(user.Id, jwtToken, refreshToken, expiresAt);

        //todo: possible to add retries if somehow userId+refreshtoken combo already exists.
        await _userSessionRepository.AddUserSessionAsync(session, ct);

        return (jwtToken, refreshToken);
    }

    [DoesNotReturn]
    private static void ThrowInvalidCredentials()
        => throw new InvalidLoginCredentialsException("Invalid username or password");

    [DoesNotReturn]
    private static void ThrowAccessRestricted()
        => throw new AccountAccessRestrictedException("Your account has been deleted");
}