using Pandora.DTOs;
using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pandora.Models.Engine;

public interface IEngineRunner
{
	Task PreloadAsync();
	Task<EngineResult> RunAsync(IReadOnlyList<IModInfo> mods);
	Task SwitchConfigurationAsync(IEngineConfiguration config);
}