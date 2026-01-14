using HKX2E;
using Pandora.DTOs;
using Pandora.Logging;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Reactive;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace Pandora.ViewModels;

public partial class LogBoxViewModel : ViewModelBase
{
	private readonly IEngineSessionState _state;
	public IEngineSessionState State => _state;

	public IObservable<string> LogStream { get; }
	public IObservable<Unit> ClearStream { get; }

	public ViewModelActivator Activator { get; } = new();

	public LogBoxViewModel(IEngineSessionState state)
	{
		_state = state;

		LogStream = ObservableNLogTarget.LogStream
			.Buffer(TimeSpan.FromMilliseconds(100))
			.Where(x => x.Count > 0)
			.Select(msgs => string.Join(Environment.NewLine, msgs) + Environment.NewLine);

		ClearStream = ObservableNLogTarget.ClearStream;
	}
}