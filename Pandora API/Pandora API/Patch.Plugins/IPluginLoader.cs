using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Pandora.API.Patch.Plugins;

public interface IPluginLoader
{
    public Assembly? LoadPlugin(DirectoryInfo directory);

    public bool TryLoadPlugin(DirectoryInfo directory, [NotNullWhen(true)] out Assembly? plugin)
    {
        plugin = LoadPlugin(directory);
        return plugin != null;
    }
}
