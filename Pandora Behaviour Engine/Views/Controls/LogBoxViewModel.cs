// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using FluentAvalonia.UI.Controls;
using Pandora.Logging.NLogger.UI;
using Pandora.Models.Engine;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Reactive.Linq;

namespace Pandora.ViewModels;

public partial class LogBoxViewModel : ViewModelBase
{
	public IEngineSharedState State { get; }

	public IObservable<LogUiEvent> LogStream { get; }

	public LaunchElementViewModel LaunchElementVM { get; }

	[ObservableAsProperty]
	private InfoBarSeverity _infoSeverity;

	[ObservableAsProperty]
	private string _infoTitle = string.Empty;

	[ObservableAsProperty]
	private string _infoMessage = string.Empty;

	public LogBoxViewModel(IEngineSharedState state, ILogEventStream stream, LaunchElementViewModel launchElement)
	{
		State = state;
		LogStream = stream.Events;
		LaunchElementVM = launchElement;

		this.WhenAnyValue(x => x.State.EngineState)
			.Select(s => s switch
			{
				EngineState.Ready => InfoBarSeverity.Informational,
				EngineState.Preloading => InfoBarSeverity.Informational,
				EngineState.Running => InfoBarSeverity.Warning,
				EngineState.Success => InfoBarSeverity.Success,
				EngineState.Error => InfoBarSeverity.Error,
				_ => InfoBarSeverity.Informational
			})
			.ToProperty(this, x => x.InfoSeverity, out _infoSeverityHelper);

		this.WhenAnyValue(x => x.State.EngineState)
			.Select(s => s switch
			{
				EngineState.Ready => "Ready to launch",
				EngineState.Preloading => "Preloading…",
				EngineState.Running => "Running…",
				EngineState.Success => "Success",
				EngineState.Error => "Error",
				_ => "Status"
			})
			.ToProperty(this, x => x.InfoTitle, out _infoTitleHelper);

		var runningTimer =
			this.WhenAnyValue(x => x.State.EngineState)
				.Select(state =>
				{
					if (state != EngineState.Running)
						return Observable.Return(string.Empty);

					var start = DateTimeOffset.UtcNow;

					return Observable
						.Interval(TimeSpan.FromMilliseconds(100), RxApp.MainThreadScheduler)
						.Select(_ =>
						{
							var elapsed = DateTimeOffset.UtcNow - start;
							return $"Elapsed time: {elapsed:mm\\:ss\\.f}";
						});
				})
				.Switch();

		var infoMessage =
			this.WhenAnyValue(x => x.State.EngineState)
				.Select(s => s switch
				{
					EngineState.Preloading => Observable.Return("Preparing resources"),
					EngineState.Running => runningTimer,
					EngineState.Success => Observable.Return("Launch completed successfully"),
					EngineState.Error => Observable.Return("See log for details"),
					_ => Observable.Return(string.Empty)
				})
				.Switch()
				.ToProperty(this, x => x.InfoMessage, out _infoMessageHelper);

	}
}