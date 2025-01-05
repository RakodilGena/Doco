namespace Doco.Server.FilesService.Options;

internal sealed class FileStoreDbLimit
{
    public const string SectionName = "FileStoreDbLimit";
    public int SizeBytes { get; set; }
}