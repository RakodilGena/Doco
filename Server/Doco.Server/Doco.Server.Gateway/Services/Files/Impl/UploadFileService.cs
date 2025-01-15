using Doco.Server.Core;
using Doco.Server.Gateway.Exceptions.ServiceDiscovery;
using FilesGrpc;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;

namespace Doco.Server.Gateway.Services.Files.Impl;

internal sealed class UploadFileService : IUploadFileService
{
    private readonly IFileServiceUrlProvider _fileServiceUrlProvider;
    private readonly byte[] _buffer;

    public UploadFileService(IFileServiceUrlProvider fileServiceUrlProvider)
    {
        _fileServiceUrlProvider = fileServiceUrlProvider;
        _buffer = new byte[Constants.GrpcStreamChunkSize];
    }

    public async Task UploadFilesAsync(
        Guid? folderId,
        IFormFileCollection files,
        CancellationToken ct)
    {
        var serviceUrl = _fileServiceUrlProvider.GetUrl();

        using var channel = GrpcChannel.ForAddress(serviceUrl);

        var client = new FilesGrpcService.FilesGrpcServiceClient(channel);

        foreach (var file in files)
        {
            await UploadFileAsync(folderId, file, client, ct);
        }
    }

    private async Task UploadFileAsync(
        Guid? folderId,
        IFormFile file,
        FilesGrpcService.FilesGrpcServiceClient fileServiceClient,
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


                string fileName, contentType;
                string? folderIdStr;
                if (chunkIndex is 0)
                {
                    fileName = file.FileName;
                    contentType = file.ContentType;
                    folderIdStr = folderId?.ToString() ?? null;
                }
                else
                {
                    fileName = contentType = string.Empty;
                    folderIdStr = null;
                }

                var chunk = new UploadFileChunk
                {
                    FolderId = folderIdStr,
                    Bytes = ByteString.CopyFrom(_buffer, 0, readCount),
                    FileName = fileName,
                    ContentType = contentType
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
                message: $"file service exception: {rpcEx.Status.Detail}",
                innerException: rpcEx.Status.DebugException);
        }
    }
}