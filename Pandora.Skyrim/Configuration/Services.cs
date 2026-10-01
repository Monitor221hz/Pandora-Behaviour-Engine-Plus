// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using Microsoft.Extensions.DependencyInjection;
using Pandora.API.Patch.Config;
using Pandora.API.Patch.Engine.Config;
using Pandora.Core.Configuration;
using Pandora.Core.Patch.Configs;
using Pandora.Skyrim.CLI;

namespace Pandora.Skyrim.Configuration;

public static class Services
{
	public static IServiceCollection AddConfigurationServices(
		this IServiceCollection serviceCollection
	)
	{
		{
			return serviceCollection
				.AddSingleton<IEngineConfigurationService, EngineConfigurationService>()
				.AddTransient<SkyrimConfiguration>()
				.AddTransient<SkyrimDebugConfiguration>()
				.AddTransient<NemesisToPandoraConfiguration>()
				.AddTransient<PandoraTemplateConfiguration>()
				.AddSingleton<IEngineConfiguration>(sp =>
				{
					var options = sp.GetService<LaunchOptions>();
					if (options is { UseSkyrimDebug64: true })
						return sp.GetRequiredService<SkyrimDebugConfiguration>();
#if DEBUG
					return sp.GetRequiredService<SkyrimDebugConfiguration>();
#else
					return sp.GetRequiredService<SkyrimConfiguration>();
#endif
				})
				.AddSingleton<Func<NemesisToPandoraConfiguration>>(sp =>
					() => sp.GetRequiredService<NemesisToPandoraConfiguration>()
				)
				.AddSingleton<Func<SkyrimDebugConfiguration>>(sp =>
					() => sp.GetRequiredService<SkyrimDebugConfiguration>()
				)
				.AddSingleton<Func<SkyrimConfiguration>>(sp =>
					() => sp.GetRequiredService<SkyrimConfiguration>()
				)
				.AddSingleton<Func<PandoraTemplateConfiguration>>(sp =>
					() => sp.GetRequiredService<PandoraTemplateConfiguration>()
				)
				.AddSingleton<
					IEngineConfigurationFactory<NemesisToPandoraConfiguration>,
					ConstEngineConfigurationFactory<NemesisToPandoraConfiguration>
				>()
				.AddSingleton<
					IEngineConfigurationFactory<SkyrimConfiguration>,
					ConstEngineConfigurationFactory<SkyrimConfiguration>
				>()
				.AddSingleton<
					IEngineConfigurationFactory<SkyrimDebugConfiguration>,
					ConstEngineConfigurationFactory<SkyrimDebugConfiguration>
				>()
				.AddSingleton<
					IEngineConfigurationFactory<PandoraTemplateConfiguration>,
					ConstEngineConfigurationFactory<PandoraTemplateConfiguration>
				>();
		}
	}
}
