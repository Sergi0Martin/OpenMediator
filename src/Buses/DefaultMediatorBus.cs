namespace OpenMediator.Buses;

internal sealed class DefaultMediatorBus : IMediatorBus
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEnumerable<IPipeline> _pipelines;

    public DefaultMediatorBus(IServiceProvider serviceProvider, IEnumerable<IPipeline> pipelines)
    {
        _serviceProvider = serviceProvider;
        _pipelines = pipelines;
    }

    public async Task<TResponse> SendAsync<TCommand, TResponse>(TCommand request)
        where TCommand : ICommand<TResponse>
    {
        var handler = (ICommandHandler<TCommand, TResponse>?)_serviceProvider.GetService(typeof(ICommandHandler<TCommand, TResponse>));
        if (handler == null)
        {
            throw new InvalidOperationException($"Handler not found for command {typeof(TCommand).Name}");
        }

        await ExecuteMiddlewares(request, async () => await handler.HandleAsync(request));
        return await handler.HandleAsync(request);
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
        await handler.HandleAsync(command);
    }

    private async Task ExecuteMiddlewares<TCommand>(TCommand command, Func<Task> next)
        where TCommand : ICommand
    {
        var middlewares = _pipelines.SelectMany(p => p.Middlewares);
        var pipeline = middlewares.Aggregate((Func<Task>)(() => next()), (nextMiddleware, middleware) => () => middleware.ExecuteAsync(command, nextMiddleware));
        await pipeline();
    }
}
