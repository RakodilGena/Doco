global using FileServiceClient = FileService.FileService.FileServiceClient;
using Doco.Server.Gateway.Authentication.Options;

namespace Doco.Server.Gateway.Endpoints.Minimal.Files;

internal static partial class FileEndpoints
{
    public static RouteGroupBuilder MapFileEndpoints(this RouteGroupBuilder app)
    {
        var group = app
            .MapGroup("files")
            .RequireAuthorization(JwtAuthConfig.PolicyName);

        group
            .MapUploadFiles()
            .MapDownloadFile()
            .MapGetAllFiles()
            .MapGetFilesFromFolder();

        return app;
    }
}