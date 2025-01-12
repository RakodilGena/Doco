namespace Doco.Server.Gateway.Authentication.Services;

public interface IJwtTokenValuesFetcher
{
    bool IsAuthorized();
    
    Guid FetchUserId();

    string FetchRawToken();
}