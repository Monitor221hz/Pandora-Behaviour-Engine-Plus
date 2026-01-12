// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Pandora.API.Patch.Config;
using Pandora.API.Patch.Plugins;

namespace Pandora.Models.Patch.Plugins;

public static class PluginManager
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private static readonly List<IEngineConfigurationPlugin> _configurations = [];
	public static IReadOnlyList<IEngineConfigurationPlugin> EngineConfigurations => _configurations;

	public static void LoadAllPlugins(DirectoryInfo assemblyDirectory)
	{
		var pluginsDirectory = new DirectoryInfo(
			Path.Combine(assemblyDirectory.FullName, "Plugins")
		);
		if (!pluginsDirectory.Exists)
			return;

		var subDirectories = pluginsDirectory.GetDirectories();
		foreach (var pluginDirectory in subDirectories)
		{
#if DEBUG
			// only for debug. DO NOT introduce json field plugin loading to release builds
			IMetaPluginLoader metaPluginLoader = new JsonPluginLoader();

			if (!metaPluginLoader.TryLoadMetadata(pluginDirectory, out var pluginInfo))
				continue;

			var assembly = metaPluginLoader.LoadPlugin(pluginDirectory, pluginInfo);
#else
			Assembly? assembly;
			try
			{
				var pluginLoader = new PluginLoader();
				assembly = pluginLoader.LoadPlugin(pluginDirectory);
			}
			catch (Exception ex)
			{
				logger.Error($"Critical error loading plugin from {pluginDirectory.Name}: {ex}");
				continue;
			}
#endif
			if (assembly != null)
			{
				AddConfigurations(assembly);
			}
		}
	}

	private static void AddConfigurations(Assembly assembly)
	{
		foreach (Type type in assembly.GetTypes())
		{
			if (
				typeof(IEngineConfigurationPlugin).IsAssignableFrom(type)
				&& Activator.CreateInstance(type) is IEngineConfigurationPlugin plugin
			)
			{
				_configurations.Add(plugin);
			}
		}
	}
}
