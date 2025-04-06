namespace OpenMediator.Buses;

public interface IMediatorBus
{
    Task<TResponse> SendAsync<TCommand, TResponse>(TCommand request)
            where TCommand : ICommand<TResponse>;

    Task SendAsync<TCommand>(TCommand command)
        where TCommand : ICommand;
}
