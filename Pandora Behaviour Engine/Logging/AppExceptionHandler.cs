// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.Enums;
using Pandora.Logging.Extensions;
using Pandora.Paths.Contexts;
using ReactiveUI;
using System;
using System.IO;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Logging;

public class AppExceptionHandler : IAppExceptionHandler, IDisposable
{
	private readonly IEnginePathContext _pathContext;

	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	public AppExceptionHandler(IEnginePathContext pathContext)
	{
		_pathContext = pathContext;
	}

	public void Initialize()
	{
		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

		RxApp.DefaultExceptionHandler = Observer.Create<Exception>(HandleRxException);
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
		var log = BuildLog(CrashType.UnhandledException, ex?.ToString() ?? "ExceptionObject is null");
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
		var log = BuildLog(CrashType.UnobservedTaskException, e.Exception.ToString());
		WriteCrashLog("Pandora_CriticalCrash_UnobservedTaskException.log", log);
		e.SetObserved();
	}

	private void HandleRxException(Exception ex)
	{
		var content = BuildLog(CrashType.ReactiveUI, ex.ToString());
		WriteCrashLog("Pandora_CriticalCrash__RxUIException", content);
	}
	private void WriteCrashLog(string fileName, string log)
	{
		try
		{
			var dir = _pathContext.OutputFolder.FullName;
			Directory.CreateDirectory(dir);
			var outputLogPath = Path.Combine(dir, fileName);
			File.WriteAllText(outputLogPath, log, Encoding.UTF8);
		}
		catch (Exception ex)
		{
			logger.UiError($"Failed to write crash log:", ex);
		}
	}

	private string BuildLog(CrashType type, string content)
	{
		var sb = new StringBuilder();
		sb.AppendLine("[ Pandora Critical Crash Log ]");
		sb.AppendLine("=======================================");
		sb.AppendLine($"Type: {type}");
		sb.AppendLine($"Environment.CurrentDirectory: {_pathContext.CurrentFolder.FullName}");
		sb.AppendLine(
			$"Executable Path: {_pathContext.AssemblyFolder.FullName ?? "unknown"}"
		);
		sb.AppendLine();
		sb.AppendLine(content);
		sb.AppendLine("=======================================");
		return sb.ToString();
	}

	public void Dispose()
	{
		AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
		TaskScheduler.UnobservedTaskException -= TaskScheduler_UnobservedTaskException;
	}

}
