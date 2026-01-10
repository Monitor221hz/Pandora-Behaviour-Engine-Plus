// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using GameFinder.Common;
using GameFinder.RegistryUtils;
using GameFinder.StoreHandlers.GOG;
using GameFinder.StoreHandlers.Steam;
using GameFinder.StoreHandlers.Steam.Models.ValueTypes;
using Microsoft.Extensions.DependencyInjection;
using NexusMods.Paths;
using Pandora.API.DTOs;
using Pandora.API.Patch.Config;
using Pandora.API.Patch.Engine.Config;
using Pandora.API.Patch.IOManagers;
using Pandora.API.Patch.Skyrim64;
using Pandora.API.Patch.Skyrim64.AnimData;
using Pandora.API.Patch.Skyrim64.AnimSetData;
using Pandora.API.Services;
using Pandora.Logging;
using Pandora.Models;
using Pandora.Models.Patch.Configs;
using Pandora.Models.Patch.IO.Skyrim64;
using Pandora.Models.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.AnimData;
using Pandora.Models.Patch.Skyrim64.AnimSetData;
using Pandora.Models.Patch.Skyrim64.Format.FNIS;
using Pandora.Models.Patch.Skyrim64.Format.Nemesis;
using Pandora.Models.Patch.Skyrim64.Format.Pandora;
using Pandora.Services.Skyrim;
using Pandora.ViewModels;
using Pandora.Views;
using System;

namespace Pandora.Services;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddPandoraServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<IFileSystem>(FileSystem.Shared);
		if (OperatingSystem.IsWindows())
		{
			serviceCollection.AddSingleton<IRegistry>(WindowsRegistry.Shared);
		}
		serviceCollection.AddSingleton<MainWindow>();



		serviceCollection.AddSingleton<PandoraServiceContext>(sp =>
			new PandoraServiceContext(
				sp.GetRequiredService<MainWindow>()
			)
		);
		serviceCollection.AddSingleton<LaunchOptions>(sp =>
		{
			var parser = sp.GetRequiredService<ILaunchOptionsParser>();
			return parser.Parse(Environment.GetCommandLineArgs());
		});

		serviceCollection.AddSingleton<StartupInfo>();


		serviceCollection.AddSingleton<ILoggingConfigurationService, NLogConfigurationService>();
		serviceCollection.AddSingleton<IModLoaderService, ModLoaderService>();
		serviceCollection.AddSingleton<IModService, ModService>();
		serviceCollection.AddSingleton<IModSettingsService, ModSettingsService>();
		serviceCollection.AddSingleton<IWindowStateService, WindowStateService>();
		serviceCollection.AddSingleton<IDiskDialogService>(sp =>
			new DiskDialogService(sp.GetRequiredService<MainWindow>())
		);

		serviceCollection.AddSingleton<IAppExceptionHandler, AppExceptionHandler>();
		serviceCollection.AddSingleton<ILaunchOptionsParser, LaunchOptionsParser>();

#if DEBUG
		serviceCollection.AddSingleton<IMetaDataExporter<IPackFile>, DebugPackFileExporter>();
#else
        serviceCollection.AddSingleton<IMetaDataExporter<IPackFile>, PackFileExporter>();
#endif


		serviceCollection.AddScoped<IFNISParser, FNISParser>();

		serviceCollection.AddScoped<IProjectManager, ProjectManager>();
		serviceCollection.AddScoped<IAnimDataManager, AnimDataManager>();
		serviceCollection.AddScoped<IAnimSetDataManager, AnimSetDataManager>();


		serviceCollection.AddScoped<NemesisAssembler>();
		serviceCollection.AddScoped<PandoraAssembler>();
		serviceCollection.AddScoped<PandoraBridgedAssembler>();

		serviceCollection.AddScoped<IPatchAssembler>(sp => sp.GetRequiredService<NemesisAssembler>());

		serviceCollection.AddScoped<SkyrimPatcher>();

		serviceCollection.AddSingleton<IEngineConfigurationService, EngineConfigurationService>();

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

		serviceCollection.AddSingleton<
			IEngineConfigurationFactory<SkyrimConfiguration>,
			ConstEngineConfigurationFactory<SkyrimConfiguration>
		>();
		serviceCollection.AddSingleton<
			IEngineConfigurationFactory<SkyrimDebugConfiguration>,
			ConstEngineConfigurationFactory<SkyrimDebugConfiguration>
		>();

		serviceCollection.AddSingleton<IPathResolver, SkyrimPathResolver>();
		serviceCollection.AddTransient<AHandler<SteamGame, AppId>, SteamHandler>();
		serviceCollection.AddTransient<AHandler<GOGGame, GOGGameId>, GOGHandler>();

		serviceCollection.AddSingleton<IBehaviourEngine>(sp =>
			new BehaviourEngine(
				sp.GetRequiredService<IPathResolver>(),
				sp.GetRequiredService<IEngineConfigurationFactory<SkyrimConfiguration>>().Create(),
				sp.GetRequiredService<IServiceScopeFactory>()
			)
		);

		return serviceCollection;
	}

	public static IServiceCollection AddViewModels(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<SettingsViewModel>();
		serviceCollection.AddSingleton<DataGridOptionsViewModel>();

		serviceCollection.AddSingleton<AboutDialogViewModel>();

		serviceCollection.AddSingleton<EngineViewModel>();
		serviceCollection.AddSingleton<MainWindowViewModel>();
		return serviceCollection;
	}
}