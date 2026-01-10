// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using NLog;
using System;

namespace Pandora.Utils;

public static class LoggerExtensions
{
	extension (Logger logger)
	{
		public void UiInfo(string message)
		{
			logger.WithProperty("ui", true).Info(message);
		}

		public void UiWarn(string message)
		{
			logger.WithProperty("ui", true).Warn(message);
		}

		public void UiError(string message, Exception? ex = null)
		{
			if (ex is not null)
			{
				logger.WithProperty("ui", true).Error(ex, message);
			}
			else
			{
				logger.WithProperty("ui", true).Error(message);
			}
		}

		public void UiClear()
		{
			logger
				.WithProperty("ui", true)
				.WithProperty("ui_command", "clear")
				.Info("--- Log Cleared ---");

		}
	}
}
