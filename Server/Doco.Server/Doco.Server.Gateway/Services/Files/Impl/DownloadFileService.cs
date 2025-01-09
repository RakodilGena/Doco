using Doco.Server.Gateway.Exceptions;
using Doco.Server.Gateway.Models.Responses.Files;
using FileService;
using Grpc.Core;
using Grpc.Net.Client;

namespace Doco.Server.Gateway.Services.Files.Impl;

internal sealed class DownloadFileService : IDownloadFileService
{
    private readonly IFileServiceUrlProvider _fileServiceUrlProvider;

    public DownloadFileService(IFileServiceUrlProvider fileServiceUrlProvider)
    {
        _fileServiceUrlProvider = fileServiceUrlProvider;
    }

    public async Task<FileDownloadResultDto> DownloadFileAsync(
        Guid fileId,
        CancellationToken ct)
    {
        var serviceUrl = _fileServiceUrlProvider.GetUrl();

        using var channel = GrpcChannel.ForAddress(serviceUrl);

        var client = new FileServiceClient(channel);

        var result = await DownloadFileAsync(fileId, client, ct);

        return result;
    }

    private static async Task<FileDownloadResultDto> DownloadFileAsync(
        Guid fileId,
        FileServiceClient client,
        CancellationToken ct)
    {
        try
        {
            var request = new DownloadFileRequest
            {
                FileId = fileId.ToString()
            };

            using var clientCall = client.DownloadFile(request, cancellationToken: ct);
            var responseStream = clientCall.ResponseStream;

            var memStream = new MemoryStream();
            memStream.Position = 0;

            var filename = string.Empty;
            var contentType = string.Empty;
            var chunkIndex = 0;

            while (await responseStream.MoveNext(ct))
            {
                var currentChunk = responseStream.Current;
                if (chunkIndex is 0)
                {
                    filename = currentChunk.FileName;
                    contentType = currentChunk.ContentType;
                }

                currentChunk.Bytes.WriteTo(memStream);
                chunkIndex++;
            }

            var fileContainer = new FileDownloadResultDto(
                memStream,
                filename,
                contentType);

            return fileContainer;
        }
        catch (RpcException rpcEx) when (rpcEx.StatusCode is StatusCode.Unavailable)
        {
            throw new ServiceUnavailableException(
                message: $"file service is unavailable: {rpcEx.Status.Detail}",
                innerException: rpcEx);
        }
        catch (RpcException rpcEx)
        {
            throw new Exception(
                message: $"file service exception: {rpcEx.Status.Detail}",
                innerException: rpcEx.Status.DebugException);
        }
    }
}