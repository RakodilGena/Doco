using FileService;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

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
        return base.GetFiles(request, context);
    }
}