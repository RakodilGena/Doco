using Doco.Server.Core;
using Doco.Server.FilesService.Models.Files;
using FileService;
using Google.Protobuf;
using Grpc.Core;

namespace Doco.Server.FilesService.Services.Grpc.Files;

internal partial class FileServiceImpl
{
    public override partial async Task DownloadFile(
        DownloadFileRequest request,
        IServerStreamWriter<DownloadFileChunk> responseStream,
        ServerCallContext context)
    {
        var fdr = BuildRequest(request);

        var container = await GetFileContainerAsync(fdr,
            context.CancellationToken);

        await StreamFileAsync(
            container,
            responseStream,
            context.CancellationToken);

        await container.Stream.DisposeAsync();
    }

    private FileDownloadRequest BuildRequest(DownloadFileRequest request)
    {
        var userId = _userFetcher.FetchUserId();
        if (userId is null)
        {
            //todo: throw valid grpc ex.
            throw new NotImplementedException();
        }

        return new FileDownloadRequest(
            userId.Value,
            Guid.Parse(request.FileId));
    }

    private Task<FileDownloadContainer> GetFileContainerAsync(
        FileDownloadRequest request,
        CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    private static async Task StreamFileAsync(
        FileDownloadContainer container,
        IServerStreamWriter<DownloadFileChunk> responseStream,
        CancellationToken ct)
    {
        var buffer = new byte[Constants.GrpcStreamChunkSize];
        var stream = container.Stream;

        var totalLength = stream.Length;

        var fileDataWritten = false;
        while (totalLength > 0)
        {
            var len = await stream.ReadAsync(buffer, ct);
            totalLength -= len;

            string fileName, contentType;
            if (fileDataWritten is false)
            {
                fileName = container.FileName;
                contentType = container.ContentType;
                fileDataWritten = true;
            }
            else
            {
                fileName = contentType = string.Empty;
            }

            await responseStream.WriteAsync(new DownloadFileChunk
            {
                Bytes = ByteString.CopyFrom(buffer, 0, len),
                FileName = fileName,
                ContentType = contentType,
            }, ct);
        }
    }
}