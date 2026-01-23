// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using GameFinder.RegistryUtils;
using Microsoft.Extensions.DependencyInjection;
using NexusMods.Paths;
using Pandora.API.Patch.Config;
using Pandora.API.Patch.Engine.Config;
using Pandora.API.Patch.IOManagers;
using Pandora.API.Patch.Skyrim64;
using Pandora.API.Patch.Skyrim64.AnimData;
using Pandora.API.Patch.Skyrim64.AnimSetData;
using Pandora.DTOs;
using Pandora.Logging.Diagnostics;
using Pandora.Logging.NLogger;
using Pandora.Logging.NLogger.Abstractions;
using Pandora.Logging.NLogger.Environment;
using Pandora.Logging.NLogger.UI;
using Pandora.Models.Engine;
using Pandora.Models.Patch.Configs;
using Pandora.Models.Patch.IO.Skyrim64;
using Pandora.Models.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.AnimData;
using Pandora.Models.Patch.Skyrim64.AnimSetData;
using Pandora.Models.Patch.Skyrim64.Format.FNIS;
using Pandora.Models.Patch.Skyrim64.Format.Nemesis;
using Pandora.Models.Patch.Skyrim64.Format.Pandora;
using Pandora.Mods.Providers;
using Pandora.Mods.Services;
using Pandora.Paths;
using Pandora.Paths.Abstractions;
using Pandora.Paths.Validation;
using Pandora.Platform.CreationEngine;
using Pandora.Platform.CreationEngine.Game;
using Pandora.Platform.CreationEngine.Locators;
using Pandora.Platform.Windows;
using Pandora.Services.Interfaces;
using Pandora.Services.Settings;
using Pandora.ViewModels;
using Pandora.Views;
using System;
using System.Linq;

namespace Pandora.Services;

public static class ServiceCollectionExtensions
{
	extension (IServiceCollection serviceCollection)
	{
		public IServiceCollection AddPandoraServices()
		{
			return serviceCollection
				.AddAppBootstrapper()
				.AddCoreServices()
				.AddSettings()
				.AddLoggingServices()
				.AddPathServices()
				.AddPatchServices()
				.AddModServices()
				.AddSkyrimServices()
				.AddGameLocators()
				.AddGameDescriptors()
				.AddBehaviourEngine()
				.AddViewModels();
		}

		private IServiceCollection AddAppBootstrapper()
		{
			return serviceCollection.AddSingleton<AppBootstrapper>();
		}

		private IServiceCollection AddViewModels()
		{
			return serviceCollection
				.AddTransient<EngineMenuViewModel>()
				.AddTransient<PatchBoxViewModel>()
				.AddTransient<LogBoxViewModel>()
				.AddTransient<LaunchElementViewModel>()
				.AddTransient<SettingsViewModel>()
				.AddTransient<DataGridOptionsViewModel>()
				.AddTransient<AboutDialogViewModel>()
				.AddTransient<EngineViewModel>()
				.AddTransient<MainWindowViewModel>();
		}

		private IServiceCollection AddGameLocators()
		{
			return serviceCollection
				.AddSingleton<GameDataValidator>()
				.AddTransient<CommandLineGameLocator>()
				.AddTransient<SteamGameLocator>()
				.AddTransient<GogGameLocator>()
				.AddTransient<RegistryGameLocator>()
				.AddSingleton<IGameLocator>(sp =>
					new CompositeGameLocator(
						[
							sp.GetRequiredService<CommandLineGameLocator>(),
							sp.GetRequiredService<SteamGameLocator>(),
							sp.GetRequiredService<GogGameLocator>(),
							sp.GetRequiredService<RegistryGameLocator>(),
						]
					)
				);
		}

		private IServiceCollection AddCoreServices()
		{
			if (OperatingSystem.IsWindows())
				serviceCollection.AddSingleton<IRegistry>(WindowsRegistry.Shared);

			return serviceCollection
				.AddSingleton<IFileSystem>(FileSystem.Shared)
				.AddSingleton<MainWindow>()
				.AddSingleton<CommandLineParser>()
				.AddSingleton<LaunchOptions>(sp =>
				{
					var parser = sp.GetRequiredService<CommandLineParser>();
					return parser.Parse([.. Environment.GetCommandLineArgs().Skip(1)]);
				})
				.AddSingleton<IDiskDialogService>(sp => new DiskDialogService(sp.GetRequiredService<MainWindow>()))
				.AddSingleton<IWindowStateService, WindowStateService>()
				.AddSingleton<IEngineSharedState, EngineSharedState>();
		}

		private IServiceCollection AddLoggingServices()
		{
			return serviceCollection
				.AddSingleton<ILogEventStream, LogEventStream>()
				.AddSingleton<ObservableNLogTarget>()
				.AddSingleton<ILogPathProvider, UserLogPathProvider>()
				.AddSingleton<INLogTargetsFactory, NLogTargetsFactory>()
				.AddSingleton<INLogConfigurator, NLogConfigurator>()
				.AddSingleton<LogFilePathUpdater>()
				.AddSingleton<LoggingBootstrapper>()
				.AddSingleton<AppExceptionHandler>()
				.AddSingleton<CrashReporter>()
				.AddSingleton<CrashLogBuilder>()
				.AddSingleton<CrashLogWriter>();
		}


		private IServiceCollection AddModServices()
		{
			return serviceCollection
				.AddSingleton<IModService, ModService>()
				.AddSingleton<IModLoaderService, ModLoaderService>()
				.AddSingleton<IModSettingsService, ModSettingsService>()
				.AddSingleton<IModInfoProvider, NemesisModInfoProvider>()
				.AddSingleton<IModInfoProvider, PandoraModInfoProvider>();
		}

		private IServiceCollection AddPatchServices()
		{
			return serviceCollection
#if DEBUG
				.AddSingleton<IMetaDataExporter<IPackFile>, DebugPackFileExporter>()
#else
				.AddSingleton<IMetaDataExporter<IPackFile>, PackFileExporter>()
#endif
				.AddScoped<IFNISParser, FNISParser>()
				.AddScoped<IProjectManager, ProjectManager>()
				.AddScoped<IAnimDataManager, AnimDataManager>()
				.AddScoped<IAnimSetDataManager, AnimSetDataManager>()
				.AddScoped<NemesisAssembler>()
				.AddScoped<PandoraAssembler>()
				.AddScoped<PandoraBridgedAssembler>()
				.AddScoped<IPatchAssembler>(sp => sp.GetRequiredService<NemesisAssembler>())
				.AddScoped<SkyrimPatcher>();
		}
		private IServiceCollection AddSkyrimServices()
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

		private IServiceCollection AddPathServices()
		{
			return serviceCollection
				.AddSingleton<IApplicationPaths, ApplicationPaths>()
				.AddSingleton<IOutputPaths, OutputPaths>()
				.AddSingleton<IUserPaths, UserPaths>()
				.AddSingleton<ISettingsRepository, SettingsRepository>()
				.AddSingleton<IEnginePathsFacade, EnginePathsFacade>()
				.AddSingleton<IGameDataValidator, GameDataValidator>();
		}

		private IServiceCollection AddGameDescriptors()
		{
			return serviceCollection.AddSingleton<IGameDescriptor, SkyrimDescriptor>();
		}

		private IServiceCollection AddSettings()
		{
			return serviceCollection
				.AddSingleton<ISettingsRepository, SettingsRepository>()
				.AddSingleton<ISettingsService, SettingsService>();
		}

		private IServiceCollection AddBehaviourEngine()
		{
			return serviceCollection
				.AddSingleton<IBehaviourEngine, BehaviourEngine>()
				.AddSingleton<IEngineStateMachine, EngineStateMachine>()
				.AddSingleton<IPatcherFactory, PatcherFactory>()
				.AddSingleton<IEngineRunner, EngineRunner>()
				.AddSingleton<EngineOrchestrator>();
		}
	}
}