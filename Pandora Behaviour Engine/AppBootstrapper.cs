// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.CLI;
using Pandora.Configuration;
using Pandora.Logging.Diagnostics;
using Pandora.Logging.NLogger;
using Pandora.Models.Engine;
using Pandora.Models.Patch.Plugins;
using Pandora.Mods.Abstractions;
using Pandora.Paths.Abstractions;
using Pandora.Platform.Windows;
using Pandora.Settings;
using Pandora.Themes;
using Pandora.Utils;
using System;
using System.Threading.Tasks;

namespace Pandora;

public sealed class AppBootstrapper(
	AppExceptionHandler appExceptionHandler,
	LoggingBootstrapper nlogger,
	IPluginManager pluginManager,
	IApplicationPaths applicationPaths,
	IEngineConfigurationService configService,
	ISettingsService settings,
	IWindowStateService windowStateService,
	Themer themer,
	IModService modService,
	IBehaviourEngine engine,
	EngineOrchestrator orchestrator,
	LaunchOptions launchOptions
)
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

	public void InitializeSync()
	{
		appExceptionHandler.Initialize();

		settings.Initialize();

		nlogger.Initialize();

		themer.Initialize();

		windowStateService.Initialize();

		var modManager = ProcessUtils.Source;

		if (ProcessUtils.IsLaunchedFromModManager)
		{
			Logger.Info("Launched from Mod Manager: {ModManager}", modManager);
		}
		else
		{
			Logger.Info("Not launched from a known Mod Manager");
		}

		orchestrator.Initialize();

		pluginManager.LoadAllPlugins(applicationPaths.AssemblyDirectory);

		configService.Initialize(launchOptions.UseSkyrimDebug64);
	}

	public async Task InitializeAsync()
	{
		try
		{
			var loadModsTask = modService.RefreshModsAsync();
			var initEngineTask = engine.InitializeAsync();

			await Task.WhenAll(loadModsTask, initEngineTask);
		}
		catch (Exception ex)
		{
			Logger.Fatal(ex, "Startup failed");
		}
	}
}