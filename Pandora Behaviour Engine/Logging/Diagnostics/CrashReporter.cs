// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿namespace Pandora.Logging.Diagnostics;

public sealed class CrashReporter(CrashLogBuilder builder, CrashLogWriter writer)
{
	public void Report(CrashType type, string content)
	{
		var log = builder.Build(type, content);
		writer.Write(type, log);
	}
}
