
namespace OpenMediator;

internal sealed class DefaultMediatorBus(IServiceProvider serviceProvider) : IMediatorBus
{
    public async Task<TResponse> SendAsync<TCommand, TResponse>(TCommand request)
        where TCommand : ICommand<TResponse>
    {
        var handler = (ICommandHandler<TCommand, TResponse>?)serviceProvider.GetService(typeof(ICommandHandler<TCommand, TResponse>));
        if (handler == null)
        {
            throw new InvalidOperationException($"Handler not found for command {typeof(TCommand).Name}");
        }
        return await handler.HandleAsync(request);
    }

    public async Task SendAsync<TCommand>(TCommand command)
        where TCommand : ICommand
    {
        var handler = (ICommandHandler<TCommand>?)serviceProvider.GetService(typeof(ICommandHandler<TCommand>));
        if (handler == null)
        {
            throw new InvalidOperationException($"Handler not found for command {typeof(TCommand).Name}");
        }
        await handler.HandleAsync(command);
    }
}
