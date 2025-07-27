namespace OpenMediator.Middlewares;

public interface IMediatorMiddleware
{
    Task ExecuteAsync<TCommand>(TCommand command, Func<Task> next, CancellationToken cancellationToken = default) where TCommand : ICommand;

    Task<TResponse> ExecuteAsync<TCommand, TResponse>(TCommand command, Func<Task<TResponse>> next, CancellationToken cancellationToken = default) where TCommand : ICommand<TResponse>;
}
