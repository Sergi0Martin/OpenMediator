namespace OpenMediator.Shared.Test.Commands;

public record CreateUserCommand(int Id, string Name) : ICommand;

public record CreateUserCommandHandler(TestDependency testDependency) : ICommandHandler<CreateUserCommand>
{
    public async Task HandleAsync(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        await Task.Delay(500, cancellationToken);
        testDependency.Call();
    }
}
