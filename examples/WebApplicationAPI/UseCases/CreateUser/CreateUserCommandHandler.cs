using OpenMediator;

namespace WebApplicationAPI.UseCases.CreateUser;

public record CreateUserCommandHandler(ILogger<CreateUserCommandHandler> _logger) : ICommandHandler<CreateUserCommand>
{
    public async Task HandleAsync(CreateUserCommand command)
    {
        // Simulate some work
        _logger.LogInformation("=========> Handling command: {Command}", command.GetType().Name);
        await Task.Delay(1000);
    }
}
