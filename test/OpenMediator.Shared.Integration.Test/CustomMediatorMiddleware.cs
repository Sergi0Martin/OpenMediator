using OpenMediator.Middlewares;

namespace OpenMediator.Shared.Integration.Test;

public class CustomMediatorMiddleware(TestDependency testDependency) : IMediatorMiddleware
{
    public async Task ExecuteAsync<TCommand>(TCommand command, Func<Task> next)
        where TCommand : ICommand
    {
        testDependency.Called = true;

        // Do something before the command
        testDependency.Counter++;
        Task.Delay(500).Wait();

        await next();

        // Do something after the command
        testDependency.Counter++;
        Task.Delay(500).Wait();
    }

    public Task ExecuteAsync<TCommand, TResponse>(TCommand command, Func<Task<TResponse>> next) where TCommand : ICommand<TResponse>
    {
        // Do something before the command
        testDependency.Counter++;
        Task.Delay(500).Wait();

        var result = next();

        // Do something after the command
        testDependency.Counter++;
        Task.Delay(500).Wait();

        return result;
    }
}