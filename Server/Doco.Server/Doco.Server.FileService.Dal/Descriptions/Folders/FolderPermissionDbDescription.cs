namespace Doco.Server.FileService.Dal.Descriptions.Folders;

internal sealed class FolderPermissionDbDescription
{
    public const string Table = "folder_permissions";

    public const string FolderId = "folder_id";
    public const string PermittedUserId = "permitted_user_id";
    public const string CreatedAt = "created_at";
    public const string CreatorId = "creator_id";
}