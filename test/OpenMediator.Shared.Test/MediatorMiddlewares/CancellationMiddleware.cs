using OpenMediator.Middlewares;

namespace OpenMediator.Shared.Test.MediatorMiddlewares;

public sealed class CancellationMiddleware : IMediatorMiddleware
{
    public Task ExecuteAsync<TCommand>(TCommand command, Func<Task> next, CancellationToken cancellationToken = default)
        where TCommand : ICommand
    {
        cancellationToken.ThrowIfCancellationRequested();
        return next();
    }

    public Task<TResponse> ExecuteAsync<TCommand, TResponse>(TCommand command, Func<Task<TResponse>> next, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResponse>
    {
        cancellationToken.ThrowIfCancellationRequested();
        return next();
    }
}