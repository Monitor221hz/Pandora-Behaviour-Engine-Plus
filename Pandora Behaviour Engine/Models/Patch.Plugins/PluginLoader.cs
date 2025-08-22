// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.Models.Patch.Engine.Plugins;
using System.IO;
using System.Reflection;

namespace Pandora.Models.Patch.Plugins;

public class PluginLoader : IPluginLoader
{
	public Assembly? LoadPlugin(DirectoryInfo directory)
	{
		FileInfo pluginInfo = new(Path.Join(directory.FullName, $"{directory.Name}.dll"));
		if (!pluginInfo.Exists) { return null; }
		PluginLoadContext loadContext = new(pluginInfo.FullName);

		return loadContext.LoadFromAssemblyName(AssemblyName.GetAssemblyName(pluginInfo.FullName));
	}

}
