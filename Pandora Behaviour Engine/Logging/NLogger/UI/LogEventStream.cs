// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Pandora.Logging.NLogger.UI;

public sealed class LogEventStream : ILogEventStream, IDisposable
{
	private readonly Channel<LogUiEvent> _channel;
	private readonly ReplaySubject<LogUiEvent> _replay;

	public LogEventStream(int channelCapacity = 10_000, int replayCapacity = 2000)
	{
		_channel = Channel.CreateBounded<LogUiEvent>(
			new BoundedChannelOptions(channelCapacity)
			{
				SingleWriter = false,
				SingleReader = false,
				FullMode = BoundedChannelFullMode.DropOldest
			});

		_replay = new ReplaySubject<LogUiEvent>(replayCapacity);

		_ = Task.Run(async () =>
		{
			await foreach (var evt in _channel.Reader.ReadAllAsync())
			{
				_replay.OnNext(evt);
			}
		});
	}

	public IObservable<LogUiEvent> Events => _replay.AsObservable();

	public bool Publish(LogUiEvent evt) => _channel.Writer.TryWrite(evt);

	public void Dispose()
	{
		_channel.Writer.TryComplete();
		_replay.OnCompleted();
		_replay.Dispose();
	}
}
