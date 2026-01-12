// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using GameFinder.RegistryUtils;
using Microsoft.Extensions.DependencyInjection;
using NexusMods.Paths;
using Pandora.API.Data;
using Pandora.API.DTOs;
using Pandora.API.Patch.Config;
using Pandora.API.Patch.Engine.Config;
using Pandora.API.Patch.IOManagers;
using Pandora.API.Patch.Skyrim64;
using Pandora.API.Patch.Skyrim64.AnimData;
using Pandora.API.Patch.Skyrim64.AnimSetData;
using Pandora.API.Services;
using Pandora.Data;
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
using Pandora.Services.CreationEngine;
using Pandora.Services.CreationEngine.Game;
using Pandora.Services.CreationEngine.Locators;
using Pandora.Services.Skyrim;
using Pandora.ViewModels;
using Pandora.Views;
using System;

namespace Pandora.Services;

public static class ServiceCollectionExtensions
{
	extension (IServiceCollection serviceCollection)
	{
		public IServiceCollection AddPandoraServices()
		{
			return serviceCollection
				.AddCoreServices()
				.AddLoggingServices()
				.AddPathServices()
				.AddPatchServices()
				.AddModServices()
				.AddSkyrimServices()
				.AddGameLocators()
				.AddGameDescriptors()
				.AddViewModels();

		}

		private IServiceCollection AddViewModels()
		{
			return serviceCollection
				.AddSingleton<SettingsViewModel>()
				.AddSingleton<DataGridOptionsViewModel>()
				.AddSingleton<AboutDialogViewModel>()
				.AddSingleton<LogViewModel>()
				.AddSingleton<EngineViewModel>()
				.AddSingleton<MainWindowViewModel>();
		}

		private IServiceCollection AddGameLocators()
		{
			return serviceCollection
				.AddTransient<CommandLineGameLocator>()
				.AddTransient<ConfigGameLocator>()
				.AddTransient<SteamGameLocator>()
				.AddTransient<GogGameLocator>()
				.AddTransient<RegistryGameLocator>()
				.AddSingleton<IGameLocator>(sp =>
				new CompositeGameLocator(
				[
					sp.GetRequiredService<CommandLineGameLocator>(),
					sp.GetRequiredService<ConfigGameLocator>(),
					sp.GetRequiredService<SteamGameLocator>(),
					sp.GetRequiredService<GogGameLocator>(),
					sp.GetRequiredService<RegistryGameLocator>(),
				])
			);
		}

		private IServiceCollection AddCoreServices()
		{
			serviceCollection.AddSingleton<IFileSystem>(FileSystem.Shared);

			if (OperatingSystem.IsWindows())
				serviceCollection.AddSingleton<IRegistry>(WindowsRegistry.Shared);

			serviceCollection.AddSingleton<MainWindow>();
			serviceCollection.AddSingleton<PandoraServiceContext>(sp =>
				new PandoraServiceContext(sp.GetRequiredService<MainWindow>())
			);
			serviceCollection.AddSingleton<ILaunchOptionsParser, LaunchOptionsParser>();
			serviceCollection.AddSingleton<LaunchOptions>(sp =>
			{
				var parser = sp.GetRequiredService<ILaunchOptionsParser>();
				return parser.Parse(Environment.GetCommandLineArgs());
			});

			serviceCollection.AddSingleton<IAppExceptionHandler, AppExceptionHandler>();
			serviceCollection.AddSingleton<IDiskDialogService>(sp => new DiskDialogService(sp.GetRequiredService<MainWindow>()));

			serviceCollection.AddSingleton<IWindowStateService, WindowStateService>();

			serviceCollection.AddSingleton<IBehaviourEngine>(sp =>
				new BehaviourEngine(
					sp.GetRequiredService<IEngineConfigurationFactory<SkyrimConfiguration>>().Create(),
					sp.GetRequiredService<IServiceScopeFactory>()
				)
			);

			return serviceCollection;
		}

		private IServiceCollection AddLoggingServices()
		{
			serviceCollection.AddSingleton<ILoggingConfigurationService, NLogConfigurationService>();
			return serviceCollection;
		}

		private IServiceCollection AddModServices()
		{
			serviceCollection.AddSingleton<IModService, ModService>();
			serviceCollection.AddSingleton<IModLoaderService, ModLoaderService>();
			serviceCollection.AddSingleton<IModSettingsService, ModSettingsService>();

			serviceCollection.AddSingleton<IModInfoProvider, NemesisModInfoProvider>();
			serviceCollection.AddSingleton<IModInfoProvider, PandoraModInfoProvider>();

			return serviceCollection;
		}

		private IServiceCollection AddPatchServices()
		{
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

			return serviceCollection;
		}
		private IServiceCollection AddSkyrimServices()
		{
			serviceCollection.AddSingleton<IEngineConfigurationService, EngineConfigurationService>();
			serviceCollection.AddTransient<SkyrimConfiguration>();
			serviceCollection.AddTransient<SkyrimDebugConfiguration>();

#if DEBUG
			serviceCollection.AddTransient<IEngineConfiguration, SkyrimDebugConfiguration>();
#else
			serviceCollection.AddTransient<IEngineConfiguration, SkyrimConfiguration>();
#endif

			serviceCollection.AddSingleton<Func<SkyrimDebugConfiguration>>(sp => () => sp.GetRequiredService<SkyrimDebugConfiguration>());
			serviceCollection.AddSingleton<Func<SkyrimConfiguration>>(sp => () => sp.GetRequiredService<SkyrimConfiguration>());

			serviceCollection.AddSingleton<IEngineConfigurationFactory<SkyrimConfiguration>, ConstEngineConfigurationFactory<SkyrimConfiguration>>();
			serviceCollection.AddSingleton<IEngineConfigurationFactory<SkyrimDebugConfiguration>, ConstEngineConfigurationFactory<SkyrimDebugConfiguration>>();

			return serviceCollection;
		}

		private IServiceCollection AddPathServices()
		{
			serviceCollection.AddSingleton<IApplicationPaths, ApplicationPaths>();
			serviceCollection.AddSingleton<IPathsConfigService, PathsConfigService>();
			serviceCollection.AddSingleton<IOutputDirectoryProvider, OutputDirectoryProvider>();
			serviceCollection.AddSingleton<IPathResolver, SkyrimPathResolver>();

			return serviceCollection;
		}

		private IServiceCollection AddGameDescriptors()
		{
			serviceCollection.AddSingleton<IGameDescriptor, SkyrimDescriptor>();
			return serviceCollection;
		}
	}
}