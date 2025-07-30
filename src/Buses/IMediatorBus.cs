namespace OpenMediator.Buses;

public interface IMediatorBus
{
    Task<TResponse> SendAsync<TCommand, TResponse>(
        TCommand request, 
        CancellationToken cancellationToken = default)
            where TCommand : ICommand<TResponse>;

    Task SendAsync<TCommand>(
        TCommand command, 
        CancellationToken cancellationToken = default)
        where TCommand : ICommand;
}
