using OpenMediator.Middlewares;
using System.Diagnostics.CodeAnalysis;

namespace OpenMediator.Shared.Test.MediatorMiddlewares;

[ExcludeFromCodeCoverage]
public class CustomMediatorMiddleware(TestDependency testDependency) : IMediatorMiddleware
{
    public async Task ExecuteAsync<TCommand>(TCommand command, Func<Task> next)
        where TCommand : ICommand
    {
        // Do something before the command
        testDependency.Call();
        await Task.Delay(500);

        await next();

        // Do something after the command
        testDependency.Call();
        await Task.Delay(500);
    }

    public async Task<TResponse> ExecuteAsync<TCommand, TResponse>(TCommand command, Func<Task<TResponse>> next)
        where TCommand : ICommand<TResponse>
    {
        // Do something before the command
        testDependency.Call();
        await Task.Delay(500);

        var result = await next();

        // Do something after the command
        testDependency.Call();
        await Task.Delay(500);

        return result;
    }
}