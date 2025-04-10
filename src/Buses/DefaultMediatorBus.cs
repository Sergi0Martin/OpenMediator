using OpenMediator.Middlewares;

namespace OpenMediator.Buses;

internal sealed class DefaultMediatorBus(
    IServiceProvider _serviceProvider, 
    IEnumerable<IMediatorMiddleware> _middlewares) : IMediatorBus
{
    public async Task<TResponse> SendAsync<TCommand, TResponse>(TCommand request)
        where TCommand : ICommand<TResponse>
    {
        var handler = (ICommandHandler<TCommand, TResponse>?)_serviceProvider.GetService(typeof(ICommandHandler<TCommand, TResponse>));
        if (handler == null)
        {
            throw new InvalidOperationException($"Handler not found for command {typeof(TCommand).Name}");
        }

        return await ExecuteMiddlewares(request, async () => await handler.HandleAsync(request));
    }

    private async Task<TResponse> ExecuteMiddlewares<TCommand, TResponse>(TCommand command, Func<Task<TResponse>> next)
     where TCommand : ICommand<TResponse>
    {
        var middlewareTask = _middlewares.Aggregate(() => next(), (nextMiddleware, middleware) => () => (Task<TResponse>)middleware.ExecuteAsync(command, nextMiddleware));
        return await middlewareTask();
    }

    public async Task SendAsync<TCommand>(TCommand command)
        where TCommand : ICommand
    {
        var handler = (ICommandHandler<TCommand>?)_serviceProvider.GetService(typeof(ICommandHandler<TCommand>));
        if (handler == null)
        {
            throw new InvalidOperationException($"Handler not found for command {typeof(TCommand).Name}");
        }

        await ExecuteMiddlewares(command, async () => await handler.HandleAsync(command));
    }

    private async Task ExecuteMiddlewares<TCommand>(TCommand command, Func<Task> next)
        where TCommand : ICommand
    {
        var middlewareTask = _middlewares.Aggregate(() => next(), (nextMiddleware, middleware) => () => middleware.ExecuteAsync(command, nextMiddleware));
        await middlewareTask();
    }
}
