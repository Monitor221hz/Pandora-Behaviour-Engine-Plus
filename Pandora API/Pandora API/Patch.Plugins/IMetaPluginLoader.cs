using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Pandora.API.Patch.Plugins;

public interface IMetaPluginLoader
{
    Assembly LoadPlugin(DirectoryInfo directory, IPluginInfo pluginInfo);

    bool TryLoadMetadata(DirectoryInfo directory, [NotNullWhen(true)] out IPluginInfo? pluginInfo);
}
