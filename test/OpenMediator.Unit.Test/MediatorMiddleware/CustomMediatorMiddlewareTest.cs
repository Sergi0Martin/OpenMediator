﻿using Microsoft.Extensions.DependencyInjection;
using OpenMediator.Buses;
using OpenMediator.Middlewares;
using OpenMediator.Shared.Test;
using OpenMediator.Shared.Test.Commands;
using OpenMediator.Shared.Test.MediatorMiddlewares;

namespace OpenMediator.Unit.Test.MediatorMiddleware;

[Collection("CommandTests")]
public sealed class CustomMediatorMiddlewareTest
{
    private ServiceCollection Services;
    private IServiceProvider _serviceProvider;
    private IMediatorBus _mediator;
    private TestDependency _testDependency;

    public CustomMediatorMiddlewareTest()
    {
        _testDependency = new TestDependency();
        Services = new ServiceCollection();
        Services.AddSingleton(_testDependency);
    }

    [Fact]
    public async Task Send_Command_With_Response_And_One_Middleware_Works()
    {
        // Arrange
        Services.AddOpenMediator(config =>
        {
            config.RegisterCommandsFromAssembly(typeof(GetUserCommandHandler).Assembly);
            config.RegisterMiddleware<CustomMediatorMiddleware>();
        });
        _serviceProvider = Services.BuildServiceProvider();
        _mediator = _serviceProvider.GetService<IMediatorBus>()!;

        var command = new GetUserCommand(1);

        // Act
        var result = await _mediator.SendAsync<GetUserCommand, User>(command);

        // Assert
        var expectedResult = new User(
            Id: 1,
            Name: "Ralph",
            Email: "ralp@company.com");
        Assert.Equal(result, expectedResult);
        Assert.True(_testDependency.Called);
        Assert.Equal(3, _testDependency.Counter);
    }

    [Fact]
    public async Task Send_Command_With_Response_And_More_Than_One_Middleware_Works()
    {
        // Arrange
        Services.AddOpenMediator(config =>
        {
            config.RegisterCommandsFromAssembly(typeof(GetUserCommandHandler).Assembly);
            config.RegisterMiddleware<CustomMediatorMiddleware>();
            config.RegisterMiddleware<SecondCustomMediatorMiddleware>();
        });
        _serviceProvider = Services.BuildServiceProvider();
        _mediator = _serviceProvider.GetService<IMediatorBus>()!;

        var command = new GetUserCommand(1);

        // Act
        var result = await _mediator.SendAsync<GetUserCommand, User>(command);

        // Assert
        var expectedResult = new User(
            Id: 1,
            Name: "Ralph",
            Email: "ralp@company.com");
        Assert.Equal(result, expectedResult);
        Assert.True(_testDependency.Called);
        Assert.Equal(5, _testDependency.Counter);
    }
    
    [Fact]
    public async Task Middleware_ShouldThrow_WhenCancellationIsRequested()
    {
        // Arrange
        var middleware = new CancellationMiddleware();
        var command = new LongRunningCommand();
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            middleware.ExecuteAsync(command, () => Task.CompletedTask, cts.Token));
    }
    
    
    [Fact]
    public async Task MediatorBus_ShouldCancel_WhenTokenIsRequested()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTransient<ICommandHandler<LongRunningCommand>, LongRunningCommandHandler>();
        var middlewares = new List<IMediatorMiddleware> { new CancellationMiddleware() };

        var serviceProvider = services.BuildServiceProvider();
        var bus = new DefaultMediatorBus(serviceProvider, middlewares);

        var command = new LongRunningCommand();
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            bus.SendAsync(command, cts.Token));
    }


}
