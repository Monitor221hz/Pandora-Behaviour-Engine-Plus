// SPDX-License-Identifier-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Microsoft.Extensions.DependencyInjection;
using Pandora.Core.Patch.Plugins;

namespace Pandora.Core.Patch;

public static class Services
{
	extension(IServiceCollection serviceCollection)
	{
		public IServiceCollection AddPatchServices()
		{
			return serviceCollection
				.AddSingleton<IPluginManager, PluginManager>()
				.AddSingleton<IPatcherFactory, PatcherFactory>();
		}
	}
}
