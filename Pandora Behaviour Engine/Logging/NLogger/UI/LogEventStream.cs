using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Channels;

namespace Pandora.Logging.NLogger.UI;

public sealed class LogEventStream : ILogEventStream
{
	private readonly Channel<LogUiEvent> _channel;

	public LogEventStream(int capacity = 10_000)
	{
		_channel = Channel.CreateBounded<LogUiEvent>(
			new BoundedChannelOptions(capacity)
			{
				SingleWriter = false,
				SingleReader = false,
				FullMode = BoundedChannelFullMode.DropOldest
			});
	}

	public IObservable<LogUiEvent> Events =>
		_channel.Reader
			.ReadAllAsync()
			.ToObservable()
			.Publish()
			.RefCount();

	public bool Publish(LogUiEvent evt)
		=> _channel.Writer.TryWrite(evt);

	public void Dispose()
		=> _channel.Writer.TryComplete();
}
