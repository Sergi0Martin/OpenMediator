namespace OpenMediator.Middlewares;

public interface IMediatorMiddleware
{
    Task ExecuteAsync<TCommand>(TCommand command, Func<Task> next) where TCommand : ICommand;

    Task<TResponse> ExecuteAsync<TCommand, TResponse>(TCommand command, Func<Task<TResponse>> next) where TCommand : ICommand<TResponse>;
}
