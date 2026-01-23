// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Microsoft.Extensions.DependencyInjection;
using Pandora.API.Patch.Config;
using Pandora.API.Patch.Engine.Config;
using Pandora.Models.Patch.Configs;
using System;

namespace Pandora.Configuration;

public static class Services
{
	extension(IServiceCollection serviceCollection)
	{
		public IServiceCollection AddConfigurationServices()
		{
			return serviceCollection
				.AddSingleton<IEngineConfigurationService, EngineConfigurationService>()
				.AddTransient<SkyrimConfiguration>()
				.AddTransient<SkyrimDebugConfiguration>()
#if DEBUG
				.AddTransient<IEngineConfiguration, SkyrimDebugConfiguration>()
#else
				.AddTransient<IEngineConfiguration, SkyrimConfiguration>()
#endif
				.AddSingleton<Func<SkyrimDebugConfiguration>>(sp => () => sp.GetRequiredService<SkyrimDebugConfiguration>())
				.AddSingleton<Func<SkyrimConfiguration>>(sp => () => sp.GetRequiredService<SkyrimConfiguration>())
				.AddSingleton<IEngineConfigurationFactory<SkyrimConfiguration>, ConstEngineConfigurationFactory<SkyrimConfiguration>>()
				.AddSingleton<IEngineConfigurationFactory<SkyrimDebugConfiguration>, ConstEngineConfigurationFactory<SkyrimDebugConfiguration>>();
		}
	}
}
