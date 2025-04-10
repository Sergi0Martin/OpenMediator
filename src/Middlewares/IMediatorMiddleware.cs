namespace OpenMediator.Middlewares;

public interface IMediatorMiddleware
{
    Task ExecuteAsync<TCommand>(TCommand command, Func<Task> next) where TCommand : ICommand;
}
