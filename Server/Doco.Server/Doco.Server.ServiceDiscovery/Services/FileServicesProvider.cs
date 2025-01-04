using Doco.Server.ServiceDiscovery.Options;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Options;

namespace Doco.Server.ServiceDiscovery.Services;

internal sealed class FileServicesProvider : FileServicesDiscovery.FileServicesDiscoveryBase
{
    private readonly string _fileServicesString;

    public FileServicesProvider(IOptionsSnapshot<FileServicesUrls> fileServicesOptions)
    {
        _fileServicesString = fileServicesOptions.Value.Value;
    }
    
    public override Task<FileServicesReply> Discover(Empty request, ServerCallContext context)
    {
        var availableServices = _fileServicesString.Split(';');
        
        return Task.FromResult(new FileServicesReply
        {
            Instances = { availableServices }
        });
    }
}