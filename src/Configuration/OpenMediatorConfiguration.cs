using OpenMediator.Middlewares;
using System.Reflection;

namespace OpenMediator.Configuration;

public sealed class OpenMediatorConfiguration
{
    internal List<Assembly> AssembliesToRegister { get; } = [];
    internal List<Type> MiddlewaresToRegister { get; } = [];

    [Obsolete("Use RegisterCommandsFromAssemblies instead.")]
    public OpenMediatorConfiguration RegisterServicesFromAssembly(Assembly assembly)
    {
        RegisterCommandsFromAssembly(assembly);
        return this;
    }

    public OpenMediatorConfiguration RegisterCommandsFromAssembly(Assembly assembly)
    {
        AssembliesToRegister.Add(assembly);
        return this;
    }

    [Obsolete("Use RegisterCommandsFromAssemblies instead.")]
    public OpenMediatorConfiguration RegisterServicesFromAssemblies(params Assembly[] assemblies)
    {
        RegisterCommandsFromAssemblies(assemblies);
        return this;
    }

    public OpenMediatorConfiguration RegisterCommandsFromAssemblies(params Assembly[] assemblies)
    {
        AssembliesToRegister.AddRange(assemblies);
        return this;
    }

    public OpenMediatorConfiguration RegisterMiddleware<TMiddleware>()
        where TMiddleware : IMediatorMiddleware
    {
        MiddlewaresToRegister.Add(typeof(TMiddleware));
        return this;
    }
}
