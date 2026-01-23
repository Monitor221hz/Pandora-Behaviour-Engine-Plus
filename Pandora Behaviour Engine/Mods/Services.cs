// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Microsoft.Extensions.DependencyInjection;
using Pandora.Mods.Abstractions;
using Pandora.Mods.Providers;

namespace Pandora.Mods;

public static class Services
{
	extension(IServiceCollection serviceCollection)
	{
		public IServiceCollection AddModServices()
		{
			return serviceCollection
				.AddSingleton<IModService, ModService>()
				.AddSingleton<IModLoaderService, ModLoaderService>()
				.AddSingleton<IModSettingsService, ModSettingsService>()
				.AddSingleton<IModInfoProvider, NemesisModInfoProvider>()
				.AddSingleton<IModInfoProvider, PandoraModInfoProvider>();
		}
	}
}