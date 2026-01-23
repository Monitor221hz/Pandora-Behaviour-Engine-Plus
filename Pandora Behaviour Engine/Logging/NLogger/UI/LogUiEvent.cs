// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿namespace Pandora.Logging.NLogger.UI;

public abstract record LogUiEvent
{
	public sealed record Message(string Text) : LogUiEvent;
	public sealed record Clear : LogUiEvent;
}
