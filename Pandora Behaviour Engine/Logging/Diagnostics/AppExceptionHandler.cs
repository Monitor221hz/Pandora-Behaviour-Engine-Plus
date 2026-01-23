// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Avalonia.Threading;
using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace Pandora.Logging.Diagnostics;

public class AppExceptionHandler(CrashReporter reporter) : IDisposable
{
	private IObserver<Exception>? _previousRxHandler;

	public void Initialize()
	{
		AppDomain.CurrentDomain.UnhandledException += OnUnhandled;
		TaskScheduler.UnobservedTaskException += OnUnobservedTask;
		Dispatcher.UIThread.UnhandledException += OnUiUnhandled;
		_previousRxHandler = RxApp.DefaultExceptionHandler;
		RxApp.DefaultExceptionHandler = Observer.Create<Exception>(OnRxException);
	}

	/// <summary>
	/// Write unspecified crashes to the log.
	///
	/// Without this, if, for example, a file inside `Pandora_Engine` is missing for some reason, it will crash without even writing to the log.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void OnUnhandled(object? s, UnhandledExceptionEventArgs e)
		=> reporter.Report(
			CrashType.UnhandledException,
			e.ExceptionObject?.ToString() ?? "Unknown exception"
		);

	/// <summary>
	/// Catches exceptions when unhandled exceptions occur in async fn.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void OnUnobservedTask(object? s, UnobservedTaskExceptionEventArgs e)
	{
		reporter.Report(CrashType.UnobservedTaskException, e.Exception.ToString());
		e.SetObserved();
	}

	private void OnRxException(Exception ex) => 
		reporter.Report(CrashType.ReactiveUI, ex.ToString());

	private void OnUiUnhandled(object? s, DispatcherUnhandledExceptionEventArgs e) => 
		reporter.Report(CrashType.UiThread, e.Exception.ToString());

	public void Dispose()
	{
		AppDomain.CurrentDomain.UnhandledException -= OnUnhandled;
		TaskScheduler.UnobservedTaskException -= OnUnobservedTask;
		Dispatcher.UIThread.UnhandledException -= OnUiUnhandled;
		if (_previousRxHandler is not null)
			RxApp.DefaultExceptionHandler = _previousRxHandler;

		GC.SuppressFinalize(this);
	}

}
