using Doco.Server.Gateway.Authentication.Services;
using Doco.Server.Gateway.Dal.Repositories;

namespace Doco.Server.Gateway.Services.Auth.Impl;

internal sealed class LogoutUserService : ILogoutUserService
{
    private readonly IJwtTokenValuesFetcher _tokenValuesFetcher;
    private readonly IUserSessionRepository _sessionRepository;

    public LogoutUserService(
        IJwtTokenValuesFetcher tokenValuesFetcher,
        IUserSessionRepository sessionRepository)
    {
        _tokenValuesFetcher = tokenValuesFetcher;
        _sessionRepository = sessionRepository;
    }

    public async Task LogoutAsync()
    {
        var userId = _tokenValuesFetcher.FetchUserId();
        var jwtToken = _tokenValuesFetcher.FetchRawToken();

        await _sessionRepository.RemoveSessionAsync(userId, jwtToken);
    }
}