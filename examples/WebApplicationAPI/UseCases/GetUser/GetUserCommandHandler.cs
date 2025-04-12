using OpenMediator;
using WebApplicationAPI.Domain;

namespace WebApplicationAPI.UseCases.GetUser;

public record GetUserCommandHandler(ILogger<GetUserCommandHandler> _logger) : ICommandHandler<GetUserCommand, User>
{
    public async Task<User> HandleAsync(GetUserCommand command)
    {
        // Simulate some work
        _logger.LogInformation("=========> Handling command: {Command}", command.GetType().Name);
        await Task.Delay(1000);

        return new User(
            id: 1,
            name: "Ralph",
            email: "ralp@company.com");
    }
}
