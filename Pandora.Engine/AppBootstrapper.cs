// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Engine.Config;
using Pandora.Core.Configuration;
using Pandora.Logging.Diagnostics;
using Pandora.Core.Logging.NLogger;
using Pandora.Core.Engine;
using Pandora.Models.Engine;
using Pandora.Core.Patch.Plugins;
using Pandora.Core.Mods.Abstractions;
using Pandora.Core.Paths.Abstractions;
using Pandora.Platform.Windows;
using Pandora.Core.Settings;
using Pandora.Themes;
using Pandora.Core.Utils;
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
	IEngineConfiguration startupConfig
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

		configService.Initialize(startupConfig);
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