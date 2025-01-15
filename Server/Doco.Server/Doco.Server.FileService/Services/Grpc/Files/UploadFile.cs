using Doco.Server.FileService.Models.Files;
using FilesGrpc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Doco.Server.FileService.Services.Grpc.Files;

internal partial class FileServiceImpl
{
    public override partial async Task<Empty> UploadFile(
        IAsyncStreamReader<UploadFileChunk> requestStream,
        ServerCallContext context)
    {
        var userId = _userFetcher.FetchUserId();
        if (userId is null)
        {
            //todo: throw valid grpc ex.
            throw new NotImplementedException();
        }
        bool fileDataRead = false;
        
        string? fileName = null, contentType = null;
        Guid? folderId = null;

        using var memStream = new MemoryStream();
        memStream.Position = 0;
        
        while (await requestStream.MoveNext(context.CancellationToken))
        {
            if (fileDataRead is false)
            {
                //1st chunk contains these values.
                fileName = requestStream.Current.FileName;
                contentType = requestStream.Current.ContentType;
                folderId = requestStream.Current.FolderId is not null 
                    ? Guid.Parse(requestStream.Current.FolderId) 
                    : null;
                fileDataRead = true;
            }
            
            var bytes = requestStream.Current.Bytes;
            bytes.WriteTo(memStream);
        }

        var container = new FileUploadRequest(
            userId.Value,
            folderId,
            memStream, 
            fileName!, 
            contentType!);
        
        await SaveFileAsync(container, context.CancellationToken);
        
        return new Empty();
    }

    private Task SaveFileAsync(FileUploadRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}