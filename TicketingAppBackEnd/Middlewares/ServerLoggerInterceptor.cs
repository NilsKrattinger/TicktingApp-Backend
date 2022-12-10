using Grpc.Core;
using Grpc.Core.Interceptors;

namespace TicketingAppBackEnd.Middlewares;

public class ServerLoggerInterceptor : Interceptor
{
    private readonly ILogger<ServerLoggerInterceptor> _logger;

    public ServerLoggerInterceptor(ILogger<ServerLoggerInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        LogCall<TRequest, TResponse>(MethodType.Unary, context);

        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            // Note: The gRPC framework also logs exceptions thrown by handlers to .NET Core logging.
            _logger.LogError(ex, "Error thrown by {ContextMethod}", context.Method);

            throw;
        }
    }

    public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        ServerCallContext context,
        ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        LogCall<TRequest, TResponse>(MethodType.ClientStreaming, context);
        return base.ClientStreamingServerHandler(requestStream, context, continuation);
    }

    public override Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {

        LogCall<TRequest, TResponse>(MethodType.ServerStreaming, context);
        return base.ServerStreamingServerHandler(request, responseStream, context, continuation);
    }

    public override Task DuplexStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        LogCall<TRequest, TResponse>(MethodType.DuplexStreaming, context);
        return base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);
    }

    private void LogCall<TRequest, TResponse>(MethodType methodType, ServerCallContext context)
        where TRequest : class
        where TResponse : class
    {
        _logger.LogWarning("Starting call. Type: {MethodType}. Methode: {Method}. User-agent: {User-agent}", methodType, context.Method, context.RequestHeaders.GetValue("user-agent"));

    }
}