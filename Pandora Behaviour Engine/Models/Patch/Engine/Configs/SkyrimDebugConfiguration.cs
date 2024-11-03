using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;
using Pandora.Core.Patchers;
using Pandora.Patch.IOManagers.Skyrim;
using Pandora.Patch.Patchers.Skyrim;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Core.Engine.Configs;
public class SkyrimDebugConfiguration : IEngineConfiguration
{
	public string Name { get; } = "Skyrim SE/AE Debug";

	public string Description { get; } =
	@"Engine configuration for Skyrim SE/AE behavior files (with debug info)";

	public IPatcher Patcher { get; } = new SkyrimPatcher(new DebugPackFileExporter());

}
