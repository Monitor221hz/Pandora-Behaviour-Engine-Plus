// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Pandora.Models.Patch.Plugins;

internal class PluginLoadContext : AssemblyLoadContext
{
	private readonly AssemblyDependencyResolver _dependencyResolver;

	public PluginLoadContext(string pluginPath)
	{
		_dependencyResolver = new AssemblyDependencyResolver(pluginPath);
	}

	public PluginLoadContext(FileInfo fileInfo)
	{
		_dependencyResolver = new(fileInfo.FullName);
	}

	protected override Assembly? Load(AssemblyName assemblyName)
	{
		string? assemblyPath = _dependencyResolver.ResolveAssemblyToPath(assemblyName);
		if (assemblyPath == null)
		{
			return null;
		}
		return LoadFromAssemblyPath(assemblyPath);
	}

	protected override nint LoadUnmanagedDll(string unmanagedDllName)
	{
		string? libraryPath = _dependencyResolver.ResolveUnmanagedDllToPath(unmanagedDllName);
		if (libraryPath == null)
		{
			return nint.Zero;
		}
		return LoadUnmanagedDllFromPath(libraryPath);
	}
}
