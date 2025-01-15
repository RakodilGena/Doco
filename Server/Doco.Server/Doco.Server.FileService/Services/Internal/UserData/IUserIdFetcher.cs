namespace Doco.Server.FileService.Services.Internal.UserData;

internal interface IUserIdFetcher
{
    Guid? FetchUserId();
}