// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.Logging;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Pandora.ViewModels;

public class LogViewModel : ViewModelBase
{
	public IObservable<string> LogStream { get; }
	public IObservable<Unit> ClearStream { get; }

	public LogViewModel()
	{
		LogStream = ObservableNLogTarget.LogStream
			.Buffer(TimeSpan.FromMilliseconds(100))
			.Where(x => x.Count > 0)
			.Select(msgs => string.Join(Environment.NewLine, msgs) + Environment.NewLine);

		ClearStream = ObservableNLogTarget.ClearStream;
	}
}