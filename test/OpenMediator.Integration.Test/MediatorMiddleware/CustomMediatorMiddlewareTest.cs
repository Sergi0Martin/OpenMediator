using Microsoft.Extensions.DependencyInjection;
using OpenMediator.Buses;
using OpenMediator.Shared.Integration.Test;
using OpenMediator.Shared.Integration.Test.Commands;

namespace OpenMediator.Integration.Test.MediatorMiddleware;

public sealed class CustomMediatorMiddlewareTest
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMediatorBus _mediator;
    private TestDependency _testDependency;

    public CustomMediatorMiddlewareTest()
    {
        _testDependency = new TestDependency();
        var services = new ServiceCollection();
        services.AddOpenMediator(config =>
        {
            config.RegisterCommandsFromAssembly(typeof(GetUserCommandHandler).Assembly);
            config.RegisterMiddleware<CustomMediatorMiddleware>();
        });
        services.AddSingleton(_testDependency);
        _serviceProvider = services.BuildServiceProvider();
        _mediator = _serviceProvider.GetService<IMediatorBus>()!;
    }

    [Fact]
    public async Task Send_Command_With_Response_Works()
    {
        // Arrange
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
}
