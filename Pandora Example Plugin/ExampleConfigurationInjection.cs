using Pandora.API.Patch.Engine.Config;

namespace ExamplePlugin;

public class ExampleConfigurationInjection : IEngineConfigurationPlugin
{
	public string MenuPath { get; } = "Skyrim 64/Behavior/Patch";

	public IEngineConfigurationFactory Factory { get; } = new EngineConfigurationFactory();
}
