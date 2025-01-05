global using FileServiceClient = FileService.FileService.FileServiceClient;

namespace Doco.Server.Gateway.Endpoints.Minimal.Files;

internal static partial class FileEndpoints
{
    private const string Route = "/files";

    public static IEndpointRouteBuilder MapFileEndpoints(this IEndpointRouteBuilder app)
    {
        return app
            .MapUploadFiles()
            .MapDownloadFile()
            .MapGetAllFiles()
            .MapGetFilesFromFolder();
    }
}