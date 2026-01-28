// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using NLog;
using NLog.Filters;
using NLog.Targets;
using Pandora.Logging.NLogger.Abstractions;

namespace Pandora.Logging.NLogger;

public sealed class NLogConfigurator : INLogConfigurator
{
	public void Configure(Target fileTarget, Target uiTarget)
	{
		LogManager.Setup()
			.SetupInternalLogger(b => b
				.LogToConsole(true)
				.SetMinimumLogLevel(LogLevel.Trace))
			.LoadConfiguration(builder =>
			{
				// File Logger
				builder.ForLogger()
					.FilterDynamic(new ConditionBasedFilter
					{
						Condition = "equals('${event-properties:ui}', true)",
						Action = FilterResult.Ignore
					}, filterDefaultAction: FilterResult.Log)
					.WriteTo(fileTarget);
				// UI Logger
				builder.ForLogger()
					.FilterDynamic(new ConditionBasedFilter
					{
						Condition = "equals('${event-properties:ui}', true)",
						Action = FilterResult.Log
					})
					.WriteTo(uiTarget);
			});
	}
}
