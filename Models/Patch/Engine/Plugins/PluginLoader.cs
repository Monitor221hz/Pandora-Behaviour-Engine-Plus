using System.IO;
using System.Reflection;

namespace Pandora.Models.Patch.Engine.Plugins;

public class PluginLoader : IPluginLoader
{
	public Assembly LoadPlugin(DirectoryInfo directory)
	{
		string pluginPath = Path.Join(directory.FullName, $"{directory.Name}.dll");

		PluginLoadContext loadContext = new(pluginPath);
		
		return loadContext.LoadFromAssemblyName(AssemblyName.GetAssemblyName(pluginPath));
	}
}
