using Doco.Server.Gateway.Exceptions;
using Doco.Server.Gateway.Mappers;
using Doco.Server.Gateway.Models.Responses;
using FileService;
using Grpc.Core;
using Grpc.Net.Client;

namespace Doco.Server.Gateway.Services.Internal.Impl;

internal sealed class GetFilesService : IGetFilesService
{
    private readonly IFileServiceUrlProvider _fileServiceUrlProvider;

    public GetFilesService(IFileServiceUrlProvider fileServiceUrlProvider)
    {
        _fileServiceUrlProvider = fileServiceUrlProvider;
    }

    public async Task<GetFilesResultDto> GetFilesAsync(
        Guid? folderId,
        CancellationToken ct)
    {
        var serviceUrl = _fileServiceUrlProvider.GetUrl();

        using var channel = GrpcChannel.ForAddress(serviceUrl);

        var client = new FileServiceClient(channel);

        var result = await GetFilesAsync(folderId, client, ct);

        return result;
    }

    private static async Task<GetFilesResultDto> GetFilesAsync(
        Guid? folderId,
        FileServiceClient client,
        CancellationToken ct)
    {
        try
        {
            var request = new GetFilesRequest
            {
                FolderId = folderId?.ToString() ?? null
            };

            var results = await client.GetFilesAsync(
                request,
                cancellationToken: ct);

            if (results is null)
                return GetFilesResultDto.Empty;

            return results.ToDto();
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