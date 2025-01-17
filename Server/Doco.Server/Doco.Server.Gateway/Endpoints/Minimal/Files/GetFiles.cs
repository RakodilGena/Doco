﻿using Doco.Server.Gateway.Models.Responses.Files;
using Doco.Server.Gateway.Services.Files;

namespace Doco.Server.Gateway.Endpoints.Minimal.Files;

internal static partial class FileEndpoints
{
    private static RouteGroupBuilder MapGetAllFiles(this RouteGroupBuilder group)
    {
        group.MapGet("get", GetAllFiles);
        return group;
    }

    private static RouteGroupBuilder MapGetFilesFromFolder(this RouteGroupBuilder group)
    {
        group.MapGet("get/{{folderId:guid}}", GetFilesFromFolder);
        return group;
    }

    /// <summary>
    /// Allows user to get all accessible files.
    /// </summary>
    /// <param name="getFilesService"></param>
    /// <param name="ct"></param>
    private static Task<GetFilesResultDto> GetAllFiles(
        IGetFilesService getFilesService,
        CancellationToken ct)
        => getFilesService.GetFilesAsync(folderId: null, ct);

    /// <summary>
    /// Allows user get accessible files from folder.
    /// </summary>
    /// <param name="folderId"></param>
    /// <param name="getFilesService"></param>
    /// <param name="ct"></param>
    private static Task<GetFilesResultDto> GetFilesFromFolder(
        Guid folderId,
        IGetFilesService getFilesService,
        CancellationToken ct)
        => getFilesService.GetFilesAsync(folderId, ct);
}