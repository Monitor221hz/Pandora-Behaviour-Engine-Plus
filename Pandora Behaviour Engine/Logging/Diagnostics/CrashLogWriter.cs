using Pandora.Logging.Extensions;
using Pandora.Paths.Abstractions;
using System;
using System.IO;
using System.Text;
using Pandora.Paths.Extensions;
using NLog;

namespace Pandora.Logging.Diagnostics;

public sealed class CrashLogWriter(IUserPaths paths)
{
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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
			logger.UiError("Failed to write crash log", ex);
		}
	}

	private static string GetFileName(CrashType type) => type switch
	{
		CrashType.UnhandledException => "Pandora_CriticalCrash_UnhandledException.log",
		CrashType.UnobservedTaskException => "Pandora_CriticalCrash_UnobservedTaskException.log",
		CrashType.UiThread => "Pandora_CriticalCrash_UiThread.log",
		CrashType.ReactiveUI => "Pandora_CriticalCrash_RxUIException.log",
		_ => "Pandora_CriticalCrash_Unknown.log"
	};
}
