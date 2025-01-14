namespace Doco.Server.FilesService.Services.Internal.UserData;

internal interface IUserIdFetcher
{
    Guid? FetchUserId();
}