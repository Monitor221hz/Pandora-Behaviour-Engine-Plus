// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Pandora.Logging.NLogger.UI;
using System;

namespace Pandora.ViewModels;

public partial class LogBoxViewModel : ViewModelBase
{
	public IEngineSharedState State { get; }

	public IObservable<LogUiEvent> LogStream { get; }

	public LogBoxViewModel(IEngineSharedState state, ILogEventStream stream)
	{
		State = state;

		LogStream = stream.Events;

	}
}