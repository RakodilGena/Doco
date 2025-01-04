using Doco.Server.Gateway.Models.Responses;
using FileService;
using File = FileService.File;

namespace Doco.Server.Gateway.Mappers;

internal static class GetFilesResultMapper
{
    public static GetFilesResultDto ToDto(this GetFilesReply reply)
    {
        return new GetFilesResultDto(
            reply.Files.Select(ToDto).ToArray(),
            reply.Folders.Select(ToDto).ToArray());
    }
    
    private static FileDto ToDto(this File file)
    {
        return new FileDto(new Guid(file.Id), file.Name, file.SizeBytes);
    }
    
    private static FolderDto ToDto(this Folder folder)
    {
        var files = folder.Files.Select(ToDto).ToArray();
        return new FolderDto(new Guid(folder.Id), folder.Name, files);
    }
}