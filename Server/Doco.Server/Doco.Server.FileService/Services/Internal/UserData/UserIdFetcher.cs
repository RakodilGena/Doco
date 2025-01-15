namespace Doco.Server.FileService.Services.Internal.UserData;

internal sealed class UserIdFetcher : IUserIdFetcher
{
    public Guid? FetchUserId()
    {
        //todo: return actual user GUID from headers
        return Guid.CreateVersion7();
    }
}