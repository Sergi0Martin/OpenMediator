# OpenMediator
Alternative for those who do not want to pay for a mediator implementation.

### Configuration
In your Program.cs call AddOpenMediator

```csharp
services.AddOpenMediator(config =>
{
    config.RegisterCommandsFromAssemblies([Assembly.GetExecutingAssembly()]);
});
```

### Usage
1 Create commands

```csharp
public record CreateUserCommand(int Id, string Name) : ICommand;
```

2 Call MediatorBus 

```csharp
[ApiController]
[Route("api/user")]
public class UserController(IMediatorBus _mediator) : ControllerBase
{
    [HttpPost()]
    public async Task<IActionResult> CreateUser()
    {
        var command = new CreateUserCommand(1, "UserTest");
        await _mediator.SendAsync(command);

        return Ok();
    }
}
```

3 Define your use case

```csharp
public record CreateUserCommandHandler(ILogger<CreateUserCommandHandler> _logger) : ICommandHandler<CreateUserCommand>
{
    public async Task HandleAsync(CreateUserCommand command)
    {
        // Simulate some work
        await Task.Delay(1000);
    }
}
```

### Middleware Configuration
Also you can configure and execute custom middlewares before or after the command.

1. Define your middleware by implementing the `IMiddleware` interface:

```csharp
public class CustomMediatorMiddleware(ILogger<CustomMediatorMiddleware> _logger) : IMediatorMiddleware
{
    public async Task ExecuteAsync<TCommand>(TCommand command, Func<Task> next)
        where TCommand : ICommand
    {
        // Do something before the command
        Task.Delay(1000).Wait();

        await next();

        // Do something after the command
        Task.Delay(1000).Wait();
    }
}
```

2. Register your middlewares in the `AddOpenMediator` method:

```csharp
services.AddOpenMediator(config =>
{
    config.RegisterCommandsFromAssemblies([Assembly.GetExecutingAssembly()]);
    config.RegisterMiddleware<CustomMediatorMiddleware>();
});
```