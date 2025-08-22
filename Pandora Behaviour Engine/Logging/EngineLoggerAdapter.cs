// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace Pandora.Logging;

public static class EngineLoggerAdapter
{
	private static readonly StringBuilder _logBuilder = new();
	private static readonly BehaviorSubject<string> _logSubject = new(string.Empty);

	public static IObservable<string> LogObservable => _logSubject.AsObservable();

	public static void AppendLine(string message)
	{
		_logBuilder.AppendLine(message);
		_logSubject.OnNext(_logBuilder.ToString());
	}

	public static void Clear()
	{
		_logBuilder.Clear();
		_logSubject.OnNext(string.Empty);
	}
}
