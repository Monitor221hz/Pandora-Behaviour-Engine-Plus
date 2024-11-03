using Pandora.API.Patch.Engine.Config;

namespace ExamplePlugin;

public class EngineConfigurationFactory : IEngineConfigurationFactory
{
	public string Name { get; } = "Example";

	public IEngineConfiguration? Config => new ExampleEngineConfiguration();
}
