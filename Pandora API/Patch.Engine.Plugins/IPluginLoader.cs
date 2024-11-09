using System.Reflection;

namespace Pandora.Models.Patch.Engine.Plugins;

public interface IPluginLoader
{
	Assembly LoadPlugin(DirectoryInfo directory);
}
