// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.IO;
using System.Text;
using NLog;
using Pandora.Logging.Extensions;
using Pandora.Paths.Abstractions;
using Pandora.Paths.Extensions;

namespace Pandora.Logging.Diagnostics;

public sealed class CrashLogWriter(IUserPaths paths)
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public void Write(CrashType type, string log)
	{
		try
		{
			var dir = paths.Output.FullName;
			Directory.CreateDirectory(dir);
			var file = dir / GetFileName(type);
			File.WriteAllText(file, log, Encoding.UTF8);
		}
		catch (Exception ex)
		{
			Logger.UiError("Failed to write crash log", ex);
		}
	}

	private static string GetFileName(CrashType type) =>
		type switch
		{
			CrashType.UnhandledException => "Pandora_CriticalCrash_UnhandledException.log",
			CrashType.UnobservedTaskException =>
				"Pandora_CriticalCrash_UnobservedTaskException.log",
			CrashType.UiThread => "Pandora_CriticalCrash_UiThread.log",
			CrashType.ReactiveUI => "Pandora_CriticalCrash_RxUIException.log",
			_ => "Pandora_CriticalCrash_Unknown.log",
		};
}
