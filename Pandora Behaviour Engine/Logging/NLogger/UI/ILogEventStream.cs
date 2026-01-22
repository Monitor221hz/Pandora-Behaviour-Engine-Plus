using System;

namespace Pandora.Logging.NLogger.UI;

public interface ILogEventStream : IDisposable
{
	IObservable<LogUiEvent> Events { get; }
	bool Publish(LogUiEvent evt);

}
