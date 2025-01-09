using Doco.Server.Core.Extensions;
using Doco.Server.Gateway.Options;
using Doco.Server.Gateway.Services.Files;
using Doco.Server.ServiceDiscovery;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Options;

namespace Doco.Server.Gateway.Services.Daemons;

internal sealed class ServiceDiscoveryDaemon : BackgroundService
{
    private readonly ServiceDiscoveryTimeout _timeout;
    private readonly FileServicesDiscovery.FileServicesDiscoveryClient _fileServicesDiscoveryClient;
    private readonly IFileServiceUrlProvider _fileServiceUrlProvider;

    public ServiceDiscoveryDaemon(
        IOptions<ServiceDiscoveryTimeout> timeoutOptions,
        FileServicesDiscovery.FileServicesDiscoveryClient fileServicesDiscoveryClient,
        IFileServiceUrlProvider fileServiceUrlProvider)
    {
        _fileServicesDiscoveryClient = fileServicesDiscoveryClient;
        _fileServiceUrlProvider = fileServiceUrlProvider;
        _timeout = timeoutOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            try
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    await StopAsync(stoppingToken);
                    break;
                }

                var result = await _fileServicesDiscoveryClient.DiscoverAsync(
                    request: new Empty(),
                    cancellationToken: stoppingToken);

                var urlString = string.Join(';', result.Instances);
                Console.WriteLine($"SD Daemon fetched file services urls: {urlString}");

                var urls = result.Instances.ToArray();
                _fileServiceUrlProvider.SetUrls(urls);

                var span = _timeout.Seconds.Seconds();
                await Task.Delay(span, stoppingToken);
            }
            catch (RpcException rpcEx) when (rpcEx.StatusCode is StatusCode.Unavailable)
            {
                Console.WriteLine("Service discovery failed: unavailable");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}