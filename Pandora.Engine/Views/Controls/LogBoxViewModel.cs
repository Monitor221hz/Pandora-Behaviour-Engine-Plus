// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using FluentAvalonia.UI.Controls;
using Pandora.Core.Logging.NLogger.UI;
using Pandora.Core.Engine;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using ReactiveUI.Primitives;
using ReactiveUI.Primitives.Extensions;

namespace Pandora.ViewModels;

public partial class LogBoxViewModel : ViewModelBase
{
	public IEngineSharedState State { get; }

	public IObservable<LogUiEvent> LogStream { get; }

	public LaunchElementViewModel LaunchElementVM { get; }

	[ObservableAsProperty]
	private FAInfoBarSeverity _infoSeverity;

	[ObservableAsProperty]
	private string _infoTitle = string.Empty;

	[ObservableAsProperty]
	private string _infoMessage = string.Empty;

	public LogBoxViewModel(
		IEngineSharedState state,
		ILogEventStream stream,
		LaunchElementViewModel launchElement
	)
	{
		State = state;
		LogStream = stream.Events;
		LaunchElementVM = launchElement;

		this.WhenAnyValue(x => x.State.EngineState)
			.Select(s =>
				s switch
				{
					EngineState.Ready => FAInfoBarSeverity.Informational,
					EngineState.Preloading => FAInfoBarSeverity.Informational,
					EngineState.Running => FAInfoBarSeverity.Warning,
					EngineState.Success => FAInfoBarSeverity.Success,
					EngineState.Error => FAInfoBarSeverity.Error,
					_ => FAInfoBarSeverity.Informational,
				}
			)
			.ToProperty(this, x => x.InfoSeverity, out _infoSeverityHelper);

		this.WhenAnyValue(x => x.State.EngineState)
			.Select(s =>
				s switch
				{
					EngineState.Ready => "Ready to launch",
					EngineState.Preloading => "Preloading…",
					EngineState.Running => "Running…",
					EngineState.Success => "Success",
					EngineState.Error => "Error",
					_ => "Status",
				}
			)
			.ToProperty(this, x => x.InfoTitle, out _infoTitleHelper);

		var runningTimer = this.WhenAnyValue(x => x.State.EngineState)
			.Select(state =>
			{
				if (state != EngineState.Running)
					return Observables.Return(string.Empty);

				var start = DateTimeOffset.UtcNow;

				return System
					.Reactive.Linq.Observable.Interval(TimeSpan.FromMilliseconds(100))
					.ObserveOn(RxSchedulers.MainThreadScheduler)
					.Select(_ =>
					{
						var elapsed = DateTimeOffset.UtcNow - start;
						return $"Elapsed time: {elapsed:mm\\:ss\\.f}";
					});
			})
			.Switch();

		var infoMessage = this.WhenAnyValue(x => x.State.EngineState)
			.Select(s =>
				s switch
				{
					EngineState.Preloading => Observables.Return("Preparing resources"),
					EngineState.Running => runningTimer,
					EngineState.Success => Observables.Return("Launch completed successfully"),
					EngineState.Error => Observables.Return("See log for details"),
					_ => Observables.Return(string.Empty),
				}
			)
			.Switch()
			.ToProperty(this, x => x.InfoMessage, out _infoMessageHelper);
	}
}