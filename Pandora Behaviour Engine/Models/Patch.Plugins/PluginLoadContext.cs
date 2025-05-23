using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Pandora.Models.Patch.Plugins;

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
