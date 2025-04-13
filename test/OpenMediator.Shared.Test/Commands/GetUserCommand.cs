namespace OpenMediator.Shared.Test.Commands;

public record GetUserCommand(int Id) : ICommand<User>;

public record User(int Id, string Name, string Email);

public record GetUserCommandHandler(TestDependency testDependency) : ICommandHandler<GetUserCommand, User>
{
    public async Task<User> HandleAsync(GetUserCommand command)
    {
        await Task.Delay(500);
        testDependency.Call();

        return new User(
            Id: 1,
            Name: "Ralph",
            Email: "ralp@company.com");
    }
}
