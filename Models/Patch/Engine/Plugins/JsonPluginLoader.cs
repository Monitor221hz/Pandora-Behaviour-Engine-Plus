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

/// <summary>
/// Retired for safety reasons. DO NOT USE UNLESS DEBUGGING.
/// </summary>
public class JsonPluginLoader : IMetaPluginLoader
{
	private static  JsonSerializerOptions jsonOptions = new()
	{ 
		AllowTrailingCommas = true,
		PropertyNameCaseInsensitive = true
	};

    public bool TryLoadMetadata(DirectoryInfo directory, [NotNullWhen(true)] out IPluginInfo? pluginInfo)
	{
		pluginInfo = null;
		FileInfo infoFile = new(Path.Join(directory.FullName, string.Concat(IPluginInfo.FILE_HEADER, ".json")));
		if (!infoFile.Exists) { return false; }
		using (var readStream = infoFile.OpenRead())
		{
			pluginInfo = JsonSerializer.Deserialize<PluginInfo>(readStream, jsonOptions);
		}
		return pluginInfo != null;
	}

	public Assembly LoadPlugin(DirectoryInfo directory, IPluginInfo pluginInfo)
	{
		string pluginPath = Path.IsPathRooted(pluginInfo.Path) ? pluginInfo.Path : Path.Join(directory.FullName, pluginInfo.Path);

		PluginLoadContext loadContext = new(pluginPath);

		return loadContext.LoadFromAssemblyName(AssemblyName.GetAssemblyName(pluginPath));
	}
}
