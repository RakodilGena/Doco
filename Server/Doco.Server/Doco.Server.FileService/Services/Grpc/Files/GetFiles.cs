using FilesGrpc;
using Grpc.Core;
using File = FilesGrpc.File;

namespace Doco.Server.FileService.Services.Grpc.Files;

internal partial class FileServiceImpl
{
    public override partial Task<GetFilesReply> GetFiles(
        GetFilesRequest request,
        ServerCallContext context)
    {
        return Task.FromResult(
            new GetFilesReply
            {
                Folders =
                {
                    new Folder
                    {
                        Id = Guid.CreateVersion7().ToString(),
                        Name = "MockFolder1"
                    }
                },
                Files =
                {
                    new File
                    {
                        Id = Guid.CreateVersion7().ToString(),
                        Name = "MockFile1"
                    },
                    new File
                    {
                        Id = Guid.CreateVersion7().ToString(),
                        Name = "MockFile2"
                    }
                }
            });
    }
}