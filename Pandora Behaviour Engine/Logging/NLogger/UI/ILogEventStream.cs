// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;

namespace Pandora.Logging.NLogger.UI;

public interface ILogEventStream : IDisposable
{
	IObservable<LogUiEvent> Events { get; }
	bool Publish(LogUiEvent evt);
}