using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;

namespace Pandora.Models.Patch.Configs
{
	public class SkyrimConfiguration : IEngineConfiguration
	{
		public string Name { get; } = "Skyrim SE/AE";

		public string Description { get; } =
		@"Engine configuration for Skyrim SE/AE behavior files";

		public IPatcher Patcher { get; } = new SkyrimPatcher(new PackFileExporter());


	}
}
