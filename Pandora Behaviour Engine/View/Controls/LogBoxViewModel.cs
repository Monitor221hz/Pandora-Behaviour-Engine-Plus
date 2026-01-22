using Pandora.Logging.NLogger.UI;
using Pandora.Services.Interfaces;
using System;

namespace Pandora.ViewModels;

public partial class LogBoxViewModel : ViewModelBase
{
	public IEngineSharedState State { get; }

	public IObservable<LogUiEvent> LogStream { get; }

	public LogBoxViewModel(IEngineSharedState state, ILogEventStream stream)
	{
		State = state;

		LogStream = stream.Events;

	}
}