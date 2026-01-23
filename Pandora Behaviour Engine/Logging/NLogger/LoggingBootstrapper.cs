// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using NLog;
using Pandora.Logging.NLogger.Abstractions;
using Pandora.Logging.NLogger.Environment;
using Pandora.Paths.Extensions;
using System;

namespace Pandora.Logging.NLogger;

public sealed class LoggingBootstrapper(
	LogFilePathUpdater updater,
	ILogPathProvider paths,
	INLogTargetsFactory targetsFactory,
	INLogConfigurator configurator) : IDisposable
{
	public void Initialize()
    {
        var fileTarget = targetsFactory.CreateFileTarget(
			"EngineLog",
            paths.Current.FullName / "Engine.log");

        var uiTarget = targetsFactory.CreateUiTarget("UiLog");

        configurator.Configure(fileTarget, uiTarget);

		LogManager.Configuration.Variables["EngineLogDir"] = paths.Current.FullName;

		LogManager.ReconfigExistingLoggers();

		updater.Enable();
	}

    public void Dispose()
    {
        updater?.Dispose();
        LogManager.Shutdown();
    }
}
