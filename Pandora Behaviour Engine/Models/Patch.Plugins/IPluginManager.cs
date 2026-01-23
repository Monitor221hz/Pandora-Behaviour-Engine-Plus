using Pandora.API.Patch.Config;
using System.Collections.Generic;
using System.IO;

namespace Pandora.Models.Patch.Plugins;

public interface IPluginManager
{
	IReadOnlyList<IEngineConfigurationPlugin> EngineConfigurationPlugins { get; }
	void LoadAllPlugins(DirectoryInfo assemblyDirectory);
}
