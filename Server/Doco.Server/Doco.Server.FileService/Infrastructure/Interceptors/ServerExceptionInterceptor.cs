using System.ComponentModel.DataAnnotations;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Doco.Server.FileService.Infrastructure.Interceptors;

internal sealed class ServerExceptionInterceptor : Interceptor
{
    //todo: add logger

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            var response = await base.UnaryServerHandler(request, context, continuation);
            return response;
        }
        catch (Exception e)
        {
            throw Handled(e);
        }
    }

    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            await base.ServerStreamingServerHandler(request, responseStream, context, continuation);
        }
        catch (Exception e)
        {
            throw Handled(e);
        }
    }

    public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        ServerCallContext context,
        ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            var response = await base.ClientStreamingServerHandler(requestStream, context, continuation);
            return response;
        }
        catch (Exception e)
        {
            throw Handled(e);
        }
    }

    public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            await base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);

        }
        catch (Exception e)
        {
            throw Handled(e);
        }
    }

    private static RpcException Handled(Exception exception)
    {
        switch (exception)
        {
            case RpcException ex:
                //_logger.LogError(exception, "Propagating RpcException");
                return ex;

            case ValidationException ex:
                //_logger.LogError(exception, "Validation exception");
                return new RpcException(new Status(StatusCode.InvalidArgument, detail: ex.Message));

            default:
                //_logger.LogError(exception, "Unhandled exception");
                return new RpcException(new Status(StatusCode.Internal, detail: exception.ToString()));
        }
    }
}