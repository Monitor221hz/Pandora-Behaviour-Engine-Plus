using Pandora.API.Patch.Engine.Plugins;
using System.IO;
using System.Reflection;

namespace Pandora.Models.Patch.Engine.Plugins;
public interface IPluginLoader
{
	Assembly LoadPlugin(DirectoryInfo directory, IPluginInfo pluginInfo);
	bool TryLoadMetadata(DirectoryInfo directory, out IPluginInfo? pluginInfo);
}