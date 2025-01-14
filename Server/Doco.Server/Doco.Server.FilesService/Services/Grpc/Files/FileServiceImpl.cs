using Doco.Server.FilesService.Services.Internal.UserData;
using FileService;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Doco.Server.FilesService.Services.Grpc.Files;

internal  sealed  partial class FileServiceImpl : FileService.FileService.FileServiceBase
{
    private readonly IUserIdFetcher _userFetcher;

    public FileServiceImpl(IUserIdFetcher userFetcher)
    {
        _userFetcher = userFetcher;
    }

    //todo: try catch every method throw rpc exception
    public override partial Task DownloadFile(
        DownloadFileRequest request,
        IServerStreamWriter<DownloadFileChunk> responseStream,
        ServerCallContext context);

    public override partial Task<Empty> UploadFile(
        IAsyncStreamReader<UploadFileChunk> requestStream,
        ServerCallContext context);

    public override partial Task<GetFilesReply> GetFiles(
        GetFilesRequest request,
        ServerCallContext context);
}