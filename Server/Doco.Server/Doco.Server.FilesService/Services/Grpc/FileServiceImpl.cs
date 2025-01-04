using FileService;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using File = FileService.File;

namespace Doco.Server.FilesService.Services.Grpc;

internal sealed class FileServiceImpl : FileService.FileService.FileServiceBase
{
    public override Task DownloadFile(
        DownloadFileRequest request,
        IServerStreamWriter<DownloadFileChunk> responseStream,
        ServerCallContext context)
    {
        return base.DownloadFile(request, responseStream, context);
    }

    public override Task<Empty> UploadFile(
        IAsyncStreamReader<UploadFileChunk> requestStream,
        ServerCallContext context)
    {
        return base.UploadFile(requestStream, context);
    }

    public override Task<GetFilesReply> GetFiles(Empty request, ServerCallContext context)
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