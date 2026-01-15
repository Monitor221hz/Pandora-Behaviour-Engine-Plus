using Pandora.Logging;
using Pandora.Services.Interfaces;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Pandora.ViewModels;

public partial class LogBoxViewModel : ViewModelBase
{
	public IEngineSharedState State { get; }

	public IObservable<string> LogStream { get; }
	public IObservable<Unit> ClearStream { get; }

	public LogBoxViewModel(IEngineSharedState state)
	{
		State = state;

		LogStream = ObservableNLogTarget.LogStream
			.Buffer(TimeSpan.FromMilliseconds(100))
			.Where(x => x.Count > 0)
			.Select(msgs => string.Join(Environment.NewLine, msgs) + Environment.NewLine);

		ClearStream = ObservableNLogTarget.ClearStream;
	}
}