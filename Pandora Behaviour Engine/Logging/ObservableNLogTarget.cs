// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using NLog;
using NLog.Targets;
using System.Reactive;
using System.Reactive.Subjects;

namespace Pandora.Logging;

[Target("ObservableTarget")]
public sealed class ObservableNLogTarget : TargetWithLayout
{
	public static readonly Subject<string> LogStream = new();
	public static readonly Subject<Unit> ClearStream = new();

	protected override void Write(LogEventInfo logEvent)
	{
		if (logEvent.Properties.ContainsKey("ui_command") &&
			logEvent.Properties["ui_command"]?.ToString() == "clear")
		{
			ClearStream.OnNext(Unit.Default);
			return;
		}

		string logMessage = Layout.Render(logEvent);
		LogStream.OnNext(logMessage);
	}
}