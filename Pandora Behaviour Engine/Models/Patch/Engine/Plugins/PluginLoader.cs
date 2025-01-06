using System.IO;
using System.Reflection;

namespace Pandora.Models.Patch.Engine.Plugins;

public class PluginLoader : IPluginLoader
{
	public Assembly? LoadPlugin(DirectoryInfo directory)
	{
		FileInfo pluginInfo = new FileInfo(Path.Join(directory.FullName, $"{directory.Name}.dll")); 
		if (!pluginInfo.Exists ) { return null; }
		PluginLoadContext loadContext = new(pluginInfo.FullName);
		
		return loadContext.LoadFromAssemblyName(AssemblyName.GetAssemblyName(pluginInfo.FullName));
	}

}
