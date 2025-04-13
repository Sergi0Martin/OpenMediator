using Microsoft.Extensions.DependencyInjection;
using OpenMediator.Buses;
using OpenMediator.Shared.Integration.Test;
using OpenMediator.Shared.Integration.Test.Commands;

namespace OpenMediator.Integration.Test.Commands;

public sealed class CommandTest
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMediatorBus _mediator;
    private TestDependency _testDependency;

    public CommandTest()
    {
        _testDependency = new TestDependency();
        var services = new ServiceCollection();
        services.AddOpenMediator(config =>
        {
            config.RegisterCommandsFromAssembly(typeof(GetUserCommandHandler).Assembly);
        });
        services.AddSingleton(_testDependency);
        _serviceProvider = services.BuildServiceProvider();
        _mediator = _serviceProvider.GetService<IMediatorBus>()!;
    }

    [Fact]
    public async Task Send_Command_Works()
    {
        // Arrange
        var command = new CreateUserCommand(1, "UserTest");

        // Act
        await _mediator.SendAsync(command);

        // Assert
        Assert.True(_testDependency.Called);
    }
}