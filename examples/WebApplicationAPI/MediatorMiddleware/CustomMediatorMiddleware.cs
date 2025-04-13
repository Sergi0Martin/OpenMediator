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
        await Task.Delay(1000);

        await next();

        // Do something after the command
        _logger.LogInformation("=========> After command: {Command}", command.GetType().Name);
        await Task.Delay(1000);
    }

    public Task ExecuteAsync<TCommand, TResponse>(TCommand command, Func<Task<TResponse>> next) where TCommand : ICommand<TResponse>
    {
        // Do something before the command
        _logger.LogInformation("=========> Before command: {Command}", command.GetType().Name);
        //await Task.Delay(1000);

        var result = next();

        // Do something after the command
        _logger.LogInformation("=========> After command: {Command}", command.GetType().Name);
        //await Task.Delay(1000);

        return result;
    }
}
