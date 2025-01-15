namespace Doco.Server.FileService.Dal.Descriptions.Files;

internal sealed class FileDbDescription
{
    public const string Table = "files";

    public const string Id = "id";
    public const string Name = "name";
    public const string ContentType = "content_type";
    public const string SizeBytes = "size_bytes";
    public const string PathToFile = "path_to_file";
    public const string CreatedAt = "created_at";
    public const string CreatorId = "creator_id";
    public const string LastAccessedAt = "last_accessed_at";
    public const string FolderId = "folder_id";
    public const string Path = "path";

    public const string ConstrNameFolder = "constraint_file_name_folder";
}