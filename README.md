# OpenMediator
Alternative for those who do not want to pay for a simple mediator implementation.

### Configuration
In your Program.cs call AddOpenMediator

![image](https://github.com/user-attachments/assets/d42e7ecf-df0d-49a7-b8a3-de43848d4844)

### Middleware and Pipeline Configuration
You can now configure and execute custom pipelines/middlewares before or after the command.

1. Define your middleware by implementing the `IMiddleware` interface:

```csharp
public class CustomMiddleware : IMiddleware
{
    public async Task ExecuteAsync<TCommand>(TCommand command, Func<Task> next)
        where TCommand : ICommand
    {
        // Do something before the command
        await next();
        // Do something after the command
    }
}
```

2. Define your pipeline by implementing the `IPipeline` interface:

```csharp
public class CustomPipeline : IPipeline
{
    public IEnumerable<IMiddleware> Middlewares { get; }

    public CustomPipeline(IEnumerable<IMiddleware> middlewares)
    {
        Middlewares = middlewares;
    }

    public async Task ExecuteAsync<TCommand>(TCommand command, Func<Task> next)
        where TCommand : ICommand
    {
        var pipeline = Middlewares.Aggregate((Func<Task>)(() => next()), (nextMiddleware, middleware) => () => middleware.ExecuteAsync(command, nextMiddleware));
        await pipeline();
    }
}
```

3. Register your middleware and pipeline in the `AddOpenMediator` method:

```csharp
services.AddOpenMediator(config =>
{
    config.RegisterMiddleware<CustomMiddleware>();
    config.RegisterPipeline<CustomPipeline>();
});
```

### Usage
1 Create commands

![image](https://github.com/user-attachments/assets/a3e5ccb2-40c4-4d77-a566-ac78fa5476f2)

2 Call MediatorBus 

![image](https://github.com/user-attachments/assets/1cf471ff-30a8-40c2-9397-057dd4ee0e07)

3 Define your use case

![image](https://github.com/user-attachments/assets/ef1fec0d-b79d-4f77-97e0-14dce02679cf)
