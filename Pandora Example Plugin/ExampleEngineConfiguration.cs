using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;

namespace ExamplePlugin;

public class ExampleEngineConfiguration : IEngineConfiguration
{
	public string Name { get; } = "Stuff";

	public string Description { get; } = "Does some stuff."; 

	public IPatcher Patcher { get; } = new ExamplePatcher();
}
