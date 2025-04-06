using System.Reflection;

namespace OpenMediator;

public sealed class OpenMediatorConfiguration
{
    internal List<Assembly> AssembliesToRegister { get; } = [];

    public OpenMediatorConfiguration RegisterServicesFromAssembly(Assembly assembly)
    {
        AssembliesToRegister.Add(assembly);
        return this;
    }

    public OpenMediatorConfiguration RegisterServicesFromAssemblies(params Assembly[] assemblies)
    {
        AssembliesToRegister.AddRange(assemblies);
        return this;
    }


}
