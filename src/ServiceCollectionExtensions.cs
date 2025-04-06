using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace OpenMediator;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenMediator(
        this IServiceCollection services,
        Action<OpenMediatorConfiguration> configuration)
    {
        var openMediatorConfiguration = new OpenMediatorConfiguration();
        configuration(openMediatorConfiguration);

        return services.AddOpenMediator(openMediatorConfiguration);
    }

    public static IServiceCollection AddOpenMediator(
        this IServiceCollection services,
        OpenMediatorConfiguration configuration)
    {
        if (configuration.AssembliesToRegister.Count == 0)
        {
            throw new ArgumentException("Assemblies to scan were not supplied, at least one assembly is necessary to scan for handlers.");
        }

        services.TryAddTransient<IMediatorBus, DefaultMediatorBus>();
        services.RegisterFromAssembly(configuration);

        return services;
    }
}
