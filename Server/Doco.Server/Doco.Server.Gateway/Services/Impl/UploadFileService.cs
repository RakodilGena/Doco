using Doco.Server.Core;
using Doco.Server.Gateway.Exceptions;
using FileService;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;

namespace Doco.Server.Gateway.Services.Impl;

internal sealed class UploadFileService : IUploadFileService
{
    private readonly IFileServiceUrlProvider _fileServiceUrlProvider;
    private readonly byte[] _buffer;

    public UploadFileService(IFileServiceUrlProvider fileServiceUrlProvider)
    {
        _fileServiceUrlProvider = fileServiceUrlProvider;
        _buffer = new byte[Constants.FileChunkSize];
    }

    public async Task UploadFilesAsync(IFormFileCollection files, CancellationToken ct)
    {
        var serviceUrl = _fileServiceUrlProvider.GetUrl();

        using var channel = GrpcChannel.ForAddress(serviceUrl);

        var client = new FileServiceClient(channel);

        foreach (var file in files)
        {
            await UploadFileAsync(file, client, ct);
        }
    }

    private async Task UploadFileAsync(
        IFormFile file,
        FileServiceClient fileServiceClient,
        CancellationToken ct)
    {
        try
        {
            var totalLength = file.Length;

            await using var stream = file.OpenReadStream();
            using var call = fileServiceClient.UploadFile(cancellationToken: ct);

            var chunkIndex = 0;
            while (totalLength > 0)
            {
                var readCount = await stream.ReadAsync(_buffer, ct);
                totalLength -= readCount;

                string fileName = chunkIndex is 0
                    ? file.FileName
                    : string.Empty;

                var chunk = new UploadFileChunk
                {
                    Bytes = ByteString.CopyFrom(_buffer, 0, readCount),
                    FileName = fileName,
                };

                await call.RequestStream.WriteAsync(chunk, ct);

                chunkIndex++;
            }

            await call.RequestStream.CompleteAsync();

            await call;
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
                message: $"analytics export service exception: {rpcEx.Status.Detail}",
                innerException: rpcEx.Status.DebugException);
        }
    }
}