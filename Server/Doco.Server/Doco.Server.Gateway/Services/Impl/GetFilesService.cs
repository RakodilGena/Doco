using Doco.Server.Gateway.Mappers;
using Doco.Server.Gateway.Models.Responses;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

namespace Doco.Server.Gateway.Services.Impl;

internal sealed class GetFilesService : IGetFilesService
{
    private readonly IFileServiceUrlProvider _fileServiceUrlProvider;

    public GetFilesService(IFileServiceUrlProvider fileServiceUrlProvider)
    {
        _fileServiceUrlProvider = fileServiceUrlProvider;
    }

    public async Task<GetFilesResultDto> GetFilesAsync(CancellationToken ct)
    {
        var serviceUrl = _fileServiceUrlProvider.GetUrl();

        using var channel = GrpcChannel.ForAddress(serviceUrl);

        var client = new FileServiceClient(channel);

        var result = await GetFilesAsync(client, ct);

        return result;
    }

    private static async Task<GetFilesResultDto> GetFilesAsync(
        FileServiceClient client,
        CancellationToken ct)
    {
        var results = await client.GetFilesAsync(
            new Empty(),
            cancellationToken: ct);

        if (results is null)
            return GetFilesResultDto.Empty;

        return results.ToDto();
    }
}