using Pandora.API.Patch.Engine.Config;

namespace Pandora.ViewModels;

public class ConstEngineConfigurationFactory<T> : IEngineConfigurationFactory
    where T : class, IEngineConfiguration, new()
{
    public ConstEngineConfigurationFactory(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public IEngineConfiguration? Config => new T();
}
