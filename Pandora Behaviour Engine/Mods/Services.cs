// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Microsoft.Extensions.DependencyInjection;
using Pandora.Core.Mods;
using Pandora.Core.Mods.Abstractions;
using Pandora.Core.Mods.Providers;
using Pandora.Mods.Abstractions;

namespace Pandora.Mods;

public static class Services
{
	extension(IServiceCollection serviceCollection)
	{
		public IServiceCollection AddModServices()
		{
			return serviceCollection
				.AddSingleton<ModService>()
				.AddSingleton<IModService>(sp => sp.GetRequiredService<ModService>())
				.AddSingleton<IModUIService>(sp => sp.GetRequiredService<ModService>())
				.AddSingleton<IModLoaderService, ModLoaderService>()
				.AddSingleton<IModSettingsService, ModSettingsService>()
				.AddSingleton<IModInfoProvider, NemesisModInfoProvider>()
				.AddSingleton<IModInfoProvider, PandoraModInfoProvider>();
		}
	}
}
