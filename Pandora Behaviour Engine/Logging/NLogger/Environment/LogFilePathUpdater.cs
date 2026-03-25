// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using NLog;
using System;
using System.IO;
using System.Reactive.Linq;

namespace Pandora.Logging.NLogger.Environment;

public sealed class LogFilePathUpdater(ILogPathProvider paths) : IDisposable
{
	private IDisposable? _subscription;

	private const string LogDirVar = "EngineLogDir";

	public void Enable()
	{
		if (_subscription is not null) return;

		_subscription = paths.Changed.Subscribe(Update);

		Update(paths.Current);
	}

	private static void Update(DirectoryInfo folder)
	{
		var config = LogManager.Configuration;
		if (config is null) return;

		var currentPath = config.Variables.TryGetValue(LogDirVar, out var val)
			? val?.ToString()
			: null;

		if (string.Equals(currentPath, folder.FullName, StringComparison.OrdinalIgnoreCase))
			return;

		config.Variables[LogDirVar] = folder.FullName;

		LogManager.ReconfigExistingLoggers();
	}

	public void Dispose()
	{
		_subscription?.Dispose();
	}
}