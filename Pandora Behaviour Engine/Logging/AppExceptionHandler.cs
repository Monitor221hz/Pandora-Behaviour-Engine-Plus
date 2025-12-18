// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Pandora.Utils;
using Pandora.Utils.Skyrim;

namespace Pandora.Logging;

public class AppExceptionHandler
{
	private IPathResolver _pathResolver;

	public AppExceptionHandler(IPathResolver pathResolver)
	{
		_pathResolver = pathResolver;
		Register();
	}

	public void Register()
	{
		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
	}

	/// <summary>
	/// Write unspecified crashes to the log.
	///
	/// Without this, if, for example, a file inside `Pandora_Engine` is missing for some reason, it will crash without even writing to the log.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		var ex = e.ExceptionObject as Exception;
		// NOTE:
		// When using /p:IncludeAllContentForSelfExtract=true -> EXE runs from temp dir
		// => Use `Environment.CurrentDirectory`:  Current exe dir
		// => Use `Directory.GetCurrentDirectory()`: Tmp dir! -> template read fails!
		var log = BuildLog("UnhandledException", ex?.ToString() ?? "ExceptionObject is null");
		WriteCrashLog("Pandora_CriticalCrash_UnhandledException.log", log);
	}

	/// <summary>
	/// Catches exceptions when unhandled exceptions occur in async fn.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void TaskScheduler_UnobservedTaskException(
		object? sender,
		UnobservedTaskExceptionEventArgs e
	)
	{
		var log = BuildLog("UnobservedTaskException", e.Exception.ToString());
		WriteCrashLog("Pandora_CriticalCrash_UnobservedTaskException.log", log);
		e.SetObserved();
	}

	private void WriteCrashLog(string fileName, string log)
	{
		try
		{
			//var dir = PandoraPaths.OutputPath.Exists
			//	? PandoraPaths.OutputPath.FullName
			//	: Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName)!;
			var dir = _pathResolver.GetOutputFolder().FullName;
			Directory.CreateDirectory(dir);
			var outputLogPath = Path.Combine(dir, fileName);
			File.WriteAllText(outputLogPath, log, Encoding.UTF8);
		}
		catch (Exception ex)
		{
			EngineLoggerAdapter.AppendLine($"Failed to write crash log: {ex}");
		}
	}

	private string BuildLog(string type, string content)
	{
		var sb = new StringBuilder();
		sb.AppendLine("[ Pandora Critical Crash Log ]");
		sb.AppendLine("=======================================");
		sb.AppendLine($"Type: {type}");
		sb.AppendLine($"Environment.CurrentDirectory: {Environment.CurrentDirectory}");
		sb.AppendLine(
			$"Executable Path: {Process.GetCurrentProcess().MainModule?.FileName ?? "unknown"}"
		);
		sb.AppendLine();
		sb.AppendLine(content);
		sb.AppendLine("=======================================");
		return sb.ToString();
	}
}
