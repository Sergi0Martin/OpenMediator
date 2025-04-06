using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace OpenMediator.Configuration;

internal static class DependencyInjectionService
{
    private static IServiceCollection? _serviceCollection;

    public static void RegisterFromAssembly(
        this IServiceCollection services,
        OpenMediatorConfiguration configuration)
    {
        _serviceCollection = services;

        foreach (var assembly in configuration.AssembliesToRegister)
        {
            RegisterFromAssembly(assembly);
        }
    }

    private static void RegisterFromAssembly(Assembly assembly)
    {
        var types = assembly.GetTypes();
        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))
                {
                    var commandType = @interface.GenericTypeArguments[0];
                    var responseType = @interface.GenericTypeArguments[1];
                    var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, responseType);
                    _serviceCollection!.AddTransient(handlerType, type);
                }
                else if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
                {
                    var commandType = @interface.GenericTypeArguments[0];
                    var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);
                    _serviceCollection!.AddTransient(handlerType, type);
                }
            }
        }
    }
}
