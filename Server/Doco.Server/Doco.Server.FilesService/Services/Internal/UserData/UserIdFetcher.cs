namespace Doco.Server.FilesService.Services.Internal.UserData;

internal sealed class UserIdFetcher : IUserIdFetcher
{
    public Guid? FetchUserId()
    {
        //todo: return actual user GUID from headers
        return Guid.CreateVersion7();
    }
}