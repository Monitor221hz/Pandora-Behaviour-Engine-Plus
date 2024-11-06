using Pandora.API.Patch.Engine.Plugins;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace Pandora.Models.Patch.Engine.Plugins;
public interface IMetaPluginLoader
{
	Assembly LoadPlugin(DirectoryInfo directory, IPluginInfo pluginInfo);

	bool TryLoadMetadata(DirectoryInfo directory, [NotNullWhen(true)] out IPluginInfo? pluginInfo);
}