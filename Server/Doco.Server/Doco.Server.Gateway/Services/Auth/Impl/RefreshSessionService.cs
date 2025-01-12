using System.Transactions;
using Doco.Server.Core.Extensions;
using Doco.Server.Gateway.Authentication.Services;
using Doco.Server.Gateway.Dal.Models.Sessions;
using Doco.Server.Gateway.Dal.Repositories;
using Doco.Server.Gateway.Exceptions.Auth;
using Doco.Server.Gateway.Models.Dtos.Auth;
using Doco.Server.Gateway.Models.Responses.Auth;
using Doco.Server.Gateway.Services.Transactions;

namespace Doco.Server.Gateway.Services.Auth.Impl;

internal sealed class RefreshSessionService : IRefreshSessionService
{
    private readonly IUserSessionRepository _sessionRepository;
    private readonly IJwtTokenCreator _jwtTokenCreator;
    private readonly IRefreshTokenCreator _refreshTokenCreator;

    public RefreshSessionService(
        IUserSessionRepository sessionRepository,
        IJwtTokenCreator jwtTokenCreator,
        IRefreshTokenCreator refreshTokenCreator)
    {
        _sessionRepository = sessionRepository;
        _jwtTokenCreator = jwtTokenCreator;
        _refreshTokenCreator = refreshTokenCreator;
    }

    public async Task<LoginUserResult> RefreshSessionAsync(
        RefreshSessionRequestDto request,
        CancellationToken ct)
    {
        using var tScope = TransactionScopeBuilder.Build(
            IsolationLevel.Serializable,
            timeout: 1.Seconds());

        await EnsureCanRefreshSessionAsync(request, ct);

        var (jwtToken, refreshToken) = await RefreshUserSessionAsync(request, ct);

        tScope.Complete();

        return new LoginUserResult(jwtToken, refreshToken);
    }

    private async Task EnsureCanRefreshSessionAsync(
        RefreshSessionRequestDto request,
        CancellationToken ct)
    {
        var canRefresh = await _sessionRepository.UserCanRefreshSessionAsync(
            request.UserId,
            request.RefreshToken,
            ct);

        if (canRefresh is false)
            throw new RefreshSessionException("unable to refresh session");
    }

    private async Task<(string jwtToken, string refreshToken)> RefreshUserSessionAsync(
        RefreshSessionRequestDto request,
        CancellationToken ct)
    {
        var userId = request.UserId;

        var newJwtToken = _jwtTokenCreator.CreateToken(userId);
        var (newRefreshToken, expiresAt) = _refreshTokenCreator.CreateRefreshToken();

        var session = new UserSessionToRefresh(
            userId,
            OldRefreshToken: request.RefreshToken,

            NewJwtToken: newJwtToken,
            NewRefreshToken: newRefreshToken,
            NewRefreshTokenExpiresAt: expiresAt);

        //no retries as if someone already refreshed session then just fail
        await _sessionRepository.RefreshUserSessionAsync(session, ct);

        return (newJwtToken, newRefreshToken);
    }
}