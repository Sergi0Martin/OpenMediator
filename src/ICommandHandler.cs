using System.Threading.Tasks;

namespace OpenMediator;

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task HandleAsync(TCommand command);
}

public interface ICommandHandler<in TCommand, TRespose>
    where TCommand : ICommand<TRespose>
{
    Task<TRespose> HandleAsync(TCommand command);
}
