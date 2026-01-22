// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using NLog;
using NLog.Targets;
using System;

namespace Pandora.Logging.NLogger.UI;

[Target("ObservableTarget")]
public sealed class ObservableNLogTarget(ILogEventStream stream) : TargetWithLayout
{
	protected override void Write(LogEventInfo logEvent)
	{
		if (IsClearCommand(logEvent))
		{
			stream.Publish(new LogUiEvent.Clear());
			return;
		}

		var message = Layout.Render(logEvent);
		if (!string.IsNullOrEmpty(message))
		{
			stream.Publish(new LogUiEvent.Message(message));
		}
	}

	private static bool IsClearCommand(LogEventInfo logEvent)
	{
		return logEvent.Properties.TryGetValue("ui_command", out var cmd)
			   && cmd is string s
			   && s.Equals("clear", StringComparison.OrdinalIgnoreCase);
	}
}