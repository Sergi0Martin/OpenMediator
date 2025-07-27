using System.Runtime.CompilerServices;
using OpenMediator.Middlewares;

[assembly: InternalsVisibleTo("OpenMediator.Unit.Test")]
namespace OpenMediator.Buses;
internal sealed class DefaultMediatorBus(
    IServiceProvider _serviceProvider,
    IEnumerable<IMediatorMiddleware> _middlewares) : IMediatorBus
{
    public async Task<TResponse> SendAsync<TCommand, TResponse>(TCommand request,
        CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResponse>
    {
        var handler =
            (ICommandHandler<TCommand, TResponse>?)_serviceProvider.GetService(
                typeof(ICommandHandler<TCommand, TResponse>));
        if (handler == null)
        {
            throw new InvalidOperationException($"Handler not found for command {typeof(TCommand).Name}");
        }

        return await ExecuteMiddlewares(request, async () => await handler.HandleAsync(request, cancellationToken), cancellationToken);
    }

    private async Task<TResponse> ExecuteMiddlewares<TCommand, TResponse>(TCommand command, Func<Task<TResponse>> next,
        CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResponse>
    {
        var middlewareTask = _middlewares.Aggregate(() => next(),
            (nextMiddleware, middleware) => async () =>
                await middleware.ExecuteAsync(command, nextMiddleware, cancellationToken));
        return await middlewareTask();
    }

    public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand
    {
        var handler = (ICommandHandler<TCommand>?)_serviceProvider.GetService(typeof(ICommandHandler<TCommand>));
        if (handler == null)
        {
            throw new InvalidOperationException($"Handler not found for command {typeof(TCommand).Name}");
        }

        await ExecuteMiddlewares(command, async () => await handler.HandleAsync(command, cancellationToken), cancellationToken);
    }

    private async Task ExecuteMiddlewares<TCommand>(TCommand command, Func<Task> next,
        CancellationToken cancellationToken = default)
        where TCommand : ICommand
    {
        var middlewareTask = _middlewares.Aggregate(next,
            (nextMiddleware, middleware) => () => middleware.ExecuteAsync(command, nextMiddleware, cancellationToken));
        await middlewareTask();
    }
}