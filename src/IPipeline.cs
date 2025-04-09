namespace OpenMediator;

public interface IPipeline
{
    Task ExecuteAsync<TCommand>(TCommand command, Func<Task> next)
        where TCommand : ICommand;

    IEnumerable<IMiddleware> Middlewares { get; }
}
