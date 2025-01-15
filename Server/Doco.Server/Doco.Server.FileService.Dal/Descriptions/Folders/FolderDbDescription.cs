namespace Doco.Server.FileService.Dal.Descriptions.Folders;

internal sealed class FolderDbDescription
{
    public const string Table = "folders";

    public const string Id = "id";
    public const string Name = "name";
    public const string CreatedAt = "created_at";
    public const string CreatorId = "creator_id";
    public const string ParentFolderId = "parent_folder_id";
    public const string Path = "path";

    public const string ConstrNameParentCreator = "constraint_folder_name_parent_creator";
}