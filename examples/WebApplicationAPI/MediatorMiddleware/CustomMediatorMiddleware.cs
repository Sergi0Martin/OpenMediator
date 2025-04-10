using OpenMediator;
using OpenMediator.Middlewares;

namespace WebApplicationAPI.MediatorMiddleware;

public class CustomMediatorMiddleware(ILogger<CustomMediatorMiddleware> _logger) : IMediatorMiddleware
{
    public async Task ExecuteAsync<TCommand>(TCommand command, Func<Task> next)
        where TCommand : ICommand
    {
        // Do something before the command
        _logger.LogInformation("=========> Before command: {Command}", command.GetType().Name);
        Task.Delay(1000).Wait();

        await next();

        // Do something after the command
        _logger.LogInformation("=========> After command: {Command}", command.GetType().Name);
        Task.Delay(1000).Wait();

    }
}
