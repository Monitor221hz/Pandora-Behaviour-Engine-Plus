using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Models.Patch.Engine.Plugins;
internal class PluginLoadContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver dependancyResolver;

    public PluginLoadContext(string pluginPath)
    {
        dependancyResolver = new AssemblyDependencyResolver(pluginPath);
    }
    public PluginLoadContext(FileInfo fileInfo)
    {
        dependancyResolver = new(fileInfo.FullName);
    }
    
    protected override Assembly? Load(AssemblyName assemblyName)
    {
        string? assemblyPath = dependancyResolver.ResolveAssemblyToPath(assemblyName);
        if (assemblyPath == null) { return null; }
        return LoadFromAssemblyPath(assemblyPath);
    }

    protected override nint LoadUnmanagedDll(string unmanagedDllName)
    {
        string? libraryPath = dependancyResolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        if (libraryPath == null) { return nint.Zero; }
        return LoadUnmanagedDllFromPath(libraryPath);
    }

}
