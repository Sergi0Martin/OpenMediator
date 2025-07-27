namespace OpenMediator.Shared.Test.Commands;

public sealed record LongRunningCommand : ICommand;

public sealed class LongRunningCommandHandler : ICommandHandler<LongRunningCommand>
{
    public async Task HandleAsync(LongRunningCommand command, CancellationToken cancellationToken = default)
    {
        for (var i = 0; i < 10; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(200, cancellationToken);
        }
    }
}