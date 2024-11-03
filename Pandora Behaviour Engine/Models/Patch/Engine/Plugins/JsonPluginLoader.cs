using Pandora.API.Patch.Engine.Plugins;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pandora.Models.Patch.Engine.Plugins;
public class JsonPluginLoader : IPluginLoader
{
	public bool TryLoadMetadata(DirectoryInfo directory, [NotNullWhen(true)] out IPluginInfo? pluginInfo)
	{
		pluginInfo = null;
		FileInfo infoFile = new(Path.Join(directory.FullName, string.Concat(IPluginInfo.FILE_HEADER, ".json")));
		if (!infoFile.Exists) { return false; }
		using (var readStream = infoFile.OpenRead())
		{
			pluginInfo = JsonSerializer.Deserialize<PluginInfo>(readStream);
		}
		return pluginInfo != null;
	}
	public Assembly LoadPlugin(DirectoryInfo directory, IPluginInfo pluginInfo)
	{
		string pluginPath = Path.Join(directory.FullName, pluginInfo.RelativePath);

		PluginLoadContext loadContext = new(pluginPath);

		return loadContext.LoadFromAssemblyName(AssemblyName.GetAssemblyName(pluginPath));
	}

}
