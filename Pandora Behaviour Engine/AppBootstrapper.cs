// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Pandora.CLI;
using Pandora.Configuration;
using Pandora.Logging.Diagnostics;
using Pandora.Logging.Extensions;
using Pandora.Logging.NLogger;
using Pandora.Models.Engine;
using Pandora.Models.Patch.Plugins;
using Pandora.Mods.Abstractions;
using Pandora.Paths.Abstractions;
using Pandora.Settings;
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
	IModService modService,
	IBehaviourEngine engine,
	EngineOrchestrator orchestrator,
	LaunchOptions launchOptions)
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	public void InitializeSync()
	{
		appExceptionHandler.Initialize();

		settings.Initialize();

		nlogger.Initialize();

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
			logger.Fatal(ex, "Startup failed");
		}
	}
}