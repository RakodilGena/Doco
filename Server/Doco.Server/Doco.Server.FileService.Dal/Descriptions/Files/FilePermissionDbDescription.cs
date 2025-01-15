namespace Doco.Server.FileService.Dal.Descriptions.Files;

internal sealed class FilePermissionDbDescription
{
    public const string Table = "file_permissions";

    public const string FileId = "file_id";
    public const string PermittedUserId = "permitted_user_id";
    public const string CreatedAt = "created_at";
    public const string CreatorId = "creator_id";
}