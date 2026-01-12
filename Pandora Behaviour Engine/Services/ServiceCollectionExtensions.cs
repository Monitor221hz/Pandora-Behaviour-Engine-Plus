// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using GameFinder.Common;
using GameFinder.RegistryUtils;
using GameFinder.StoreHandlers.GOG;
using GameFinder.StoreHandlers.Steam;
using GameFinder.StoreHandlers.Steam.Models.ValueTypes;
using Microsoft.Extensions.DependencyInjection;
using NexusMods.Paths;
using Pandora.API;
using Pandora.API.Patch;
using Pandora.API.Patch.Config;
using Pandora.API.Patch.Engine.Config;
using Pandora.API.Patch.IOManagers;
using Pandora.API.Patch.Skyrim64;
using Pandora.API.Patch.Skyrim64.AnimData;
using Pandora.API.Patch.Skyrim64.AnimSetData;
using Pandora.API.Services;
using Pandora.API.Utils;
using Pandora.Models;
using Pandora.Models.Patch.Configs;
using Pandora.Models.Patch.IO.Skyrim64;
using Pandora.Models.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.AnimData;
using Pandora.Models.Patch.Skyrim64.AnimSetData;
using Pandora.Models.Patch.Skyrim64.Format.FNIS;
using Pandora.Models.Patch.Skyrim64.Format.Nemesis;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using Pandora.Utils.Skyrim;
using Pandora.ViewModels;
using Pandora.ViewModels.Configuration;

namespace Pandora.Services;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddPandoraServices(
		this IServiceCollection serviceCollection,
		PandoraServiceContext context
	)
	{
		serviceCollection.AddSingleton<IFileSystem>(FileSystem.Shared);

		if (OperatingSystem.IsWindows())
		{
			serviceCollection.AddSingleton<IRegistry>(WindowsRegistry.Shared);
		}
		serviceCollection.AddSingleton<IDiskDialogService>(
			new DiskDialogService(context.MainWindow)
		);
		serviceCollection.AddSingleton<IModLoader, ModLoader>();
		serviceCollection.AddSingleton<IEngineConfigurationService, EngineConfigurationService>();
#if DEBUG
		serviceCollection.AddSingleton<IMetaDataExporter<IPackFile>, DebugPackFileExporter>();
#else
		serviceCollection.AddSingleton<IMetaDataExporter<IPackFile>, PackFileExporter>();
#endif

		serviceCollection.AddTransient<IPatcher, SkyrimPatcher>();
		serviceCollection.AddTransient<IFNISParser, FNISParser>();

		serviceCollection.AddTransient<IBehaviourEngine, BehaviourEngine>();
		serviceCollection.AddTransient<IProjectManager, ProjectManager>();
		serviceCollection.AddTransient<IPatchAssembler, NemesisAssembler>();
		serviceCollection.AddTransient<IAnimDataManager, AnimDataManager>();
		serviceCollection.AddTransient<IAnimSetDataManager, AnimSetDataManager>();

		serviceCollection.AddTransient<SkyrimConfiguration>();
		serviceCollection.AddTransient<SkyrimDebugConfiguration>();
#if DEBUG
		serviceCollection.AddTransient<IEngineConfiguration, SkyrimDebugConfiguration>();
#else
		serviceCollection.AddTransient<IEngineConfiguration, SkyrimConfiguration>();
#endif
		serviceCollection.AddSingleton<Func<SkyrimDebugConfiguration>>(sp =>
			() => sp.GetRequiredService<SkyrimDebugConfiguration>()
		);
		serviceCollection.AddSingleton<Func<SkyrimConfiguration>>(sp =>
			() => sp.GetRequiredService<SkyrimConfiguration>()
		);

		serviceCollection.AddTransient<AHandler<SteamGame, AppId>, SteamHandler>();
		serviceCollection.AddTransient<AHandler<GOGGame, GOGGameId>, GOGHandler>();
		serviceCollection.AddSingleton<IPathResolver, SkyrimPathResolver>();

		serviceCollection.AddSingleton<IEngineConfigurationService, EngineConfigurationService>();
		serviceCollection.AddSingleton<
			IEngineConfigurationFactory<SkyrimConfiguration>,
			ConstEngineConfigurationFactory<SkyrimConfiguration>
		>();
		serviceCollection.AddSingleton<
			IEngineConfigurationFactory<SkyrimDebugConfiguration>,
			ConstEngineConfigurationFactory<SkyrimDebugConfiguration>
		>();

		return serviceCollection;
	}

	public static IServiceCollection AddViewModels(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<EngineViewModel>();
		serviceCollection.AddSingleton<MainWindowViewModel>();
		return serviceCollection;
	}
}
