// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

namespace Pandora.Logging.Diagnostics;

public enum CrashType
{
	UnhandledException,
	UnobservedTaskException,
	UiThread,
	ReactiveUI
}