using Microsoft.Extensions.DependencyInjection;
using OpenMediator.Buses;
using OpenMediator.Shared.Test.Commands;

namespace OpenMediator.Shared.Test;

public class UnitTestFixture
{
    public readonly IServiceProvider ServiceProvider;
    public readonly IMediatorBus Mediator;

    public UnitTestFixture()
    {
        var testDependency = new TestDependency();
        var services = new ServiceCollection();
        services.AddOpenMediator(config =>
        {
            config.RegisterCommandsFromAssembly(typeof(CreateUserCommand).Assembly);
        });
        services.AddSingleton(testDependency);
        ServiceProvider = services.BuildServiceProvider();
        Mediator = ServiceProvider.GetService<IMediatorBus>()!;
    }
}
