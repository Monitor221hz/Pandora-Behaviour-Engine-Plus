// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Pandora.API.Patch.Config;
using Pandora.API.Patch.Plugins;

namespace Pandora.Models.Patch.Plugins;

public sealed class PluginManager : IPluginManager
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private readonly List<IEngineConfigurationPlugin> _plugins = new();
	public IReadOnlyList<IEngineConfigurationPlugin> EngineConfigurationPlugins => _plugins;

	public void LoadAllPlugins(DirectoryInfo assemblyDirectory)
	{
		if (assemblyDirectory == null || !assemblyDirectory.Exists) return;

        var pluginsDir = new DirectoryInfo(Path.Combine(assemblyDirectory.FullName, "Plugins"));
        if (!pluginsDir.Exists) 
			return;

		foreach (var pluginDir in pluginsDir.GetDirectories())
		{
			try
			{
#if DEBUG
                LoadPluginDebug(pluginDir);
#else
				LoadPluginRelease(pluginDir);
#endif
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Failed to load plugin from directory {Dir}", pluginDir.FullName);
			}
		}
	}

	/// <summary>
	/// Only for debug. DO NOT introduce json field plugin loading to release builds
	/// </summary>
	private void LoadPluginDebug(DirectoryInfo pluginDir)
	{
		IMetaPluginLoader metaLoader = new JsonPluginLoader();
		if (!metaLoader.TryLoadMetadata(pluginDir, out var pluginInfo))
		{
			logger.Warn("No metadata found for plugin {Dir}", pluginDir.FullName);
			return;
		}

		var assembly = metaLoader.LoadPlugin(pluginDir, pluginInfo);
		RegisterConfigurationsFromAssembly(assembly);
	}

	private void LoadPluginRelease(DirectoryInfo pluginDir)
	{
		var pluginDll = new FileInfo(Path.Combine(pluginDir.FullName, $"{pluginDir.Name}.dll"));
		if (!pluginDll.Exists)
		{
			logger.Warn("Plugin DLL not found: {Dll}", pluginDll.FullName);
			return;
		}

		var loader = new PluginLoadContext(pluginDll.FullName);
		var assembly = loader.LoadFromAssemblyName(AssemblyName.GetAssemblyName(pluginDll.FullName));
		RegisterConfigurationsFromAssembly(assembly);
	}

	private void RegisterConfigurationsFromAssembly(Assembly assembly)
	{
		foreach (var type in assembly.GetTypes())
		{
			if (!typeof(IEngineConfigurationPlugin).IsAssignableFrom(type)) continue;

			try
			{
				if (Activator.CreateInstance(type) is IEngineConfigurationPlugin plugin)
				{
					_plugins.Add(plugin);
					logger.Info("Loaded plugin configuration: {Plugin}", type.FullName);
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Failed to instantiate plugin type {Type}", type.FullName);
			}
		}
	}
}
