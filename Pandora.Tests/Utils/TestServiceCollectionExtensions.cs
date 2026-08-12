// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Microsoft.Extensions.DependencyInjection;
using Pandora.API.Patch.Config;
using Pandora.API.Patch.Engine.Config;
using Pandora.API.Patch.IOManagers;
using Pandora.API.Patch.Skyrim64;
using Pandora.API.Patch.Skyrim64.AnimData;
using Pandora.API.Patch.Skyrim64.AnimSetData;
using Pandora.Core.Configuration;
using Pandora.Core.Engine;
using Pandora.Core.Patch;
using Pandora.Core.Patch.Configs;
using Pandora.Skyrim.Patch.IO;
using Pandora.Skyrim;
using Pandora.Skyrim.AnimData;
using Pandora.Skyrim.AnimSetData;
using Pandora.Skyrim.Configuration;
using Pandora.Skyrim.Format.FNIS;
using Pandora.Skyrim.Format.Nemesis;
using Pandora.Skyrim.Format.Pandora;
using Pandora.Skyrim.Platform;
using Pandora.Core.Mods;
using Pandora.Core.Mods.Abstractions;
using Pandora.Core.Mods.Providers;
using Pandora.Core.Platform.CreationEngine;

namespace PandoraTests.Utils;

internal static class TestServiceCollectionExtensions
{
	public static IServiceCollection AddPandoraTestServices(this IServiceCollection services)
	{
		return services.AddTestMods().AddTestEngine().AddTestPatchPipeline();
	}

	private static IServiceCollection AddTestMods(this IServiceCollection services)
	{
		services.AddSingleton<IModLoaderService, ModLoaderService>();
		services.AddSingleton<IModSettingsService, ModSettingsService>();

		services.AddSingleton<IModInfoProvider, NemesisModInfoProvider>();
		services.AddSingleton<IModInfoProvider, PandoraModInfoProvider>();

		return services;
	}

	private static IServiceCollection AddTestPatchPipeline(this IServiceCollection services)
	{
		services.AddScoped<IFNISParser, FNISParser>();
		services.AddScoped<IProjectManager, ProjectManager>();
		services.AddScoped<IAnimDataManager, AnimDataManager>();
		services.AddScoped<IAnimSetDataManager, AnimSetDataManager>();

		services.AddScoped<NemesisAssembler>();
		services.AddScoped<PandoraAssembler>();
		services.AddScoped<PandoraBridgedAssembler>();
		services.AddScoped<SkyrimPatcher>();

		services.AddScoped<IPatchAssembler>(sp => sp.GetRequiredService<NemesisAssembler>());

		services.AddSingleton<DebugPackFileExporter>();

		services.AddSingleton<TestCapturingExporter>(sp =>
		{
			var realExporter = sp.GetRequiredService<DebugPackFileExporter>();
			return new TestCapturingExporter(realExporter);
		});

		services.AddSingleton<IMetaDataExporter<IPackFile>>(sp =>
			sp.GetRequiredService<TestCapturingExporter>()
		);

		return services;
	}

	private static IServiceCollection AddTestEngine(this IServiceCollection services)
	{
		services.AddSingleton<IEngineRunner, EngineRunner>();
		services.AddSingleton<IEngineStateMachine, EngineStateMachine>();
		services.AddSingleton<IPatcherFactory, PatcherFactory>();
		services.AddSingleton<IBehaviourEngine, BehaviourEngine>();

		services.AddSingleton<IEngineConfigurationService, EngineConfigurationService>();
		services.AddSingleton<SkyrimDebugConfiguration>();
		services.AddTransient<IEngineConfiguration>(sp =>
			sp.GetRequiredService<SkyrimDebugConfiguration>()
		);

		services.AddSingleton<Func<SkyrimDebugConfiguration>>(sp =>
			() => sp.GetRequiredService<SkyrimDebugConfiguration>()
		);
		services.AddSingleton<
			IEngineConfigurationFactory<SkyrimDebugConfiguration>,
			ConstEngineConfigurationFactory<SkyrimDebugConfiguration>
		>();

		services.AddSingleton<IGameDescriptor, SkyrimDescriptor>();

		return services;
	}
}
