using System;
using System.Reactive.Subjects;
using System.Text;

namespace Pandora.Utils;

public static class EngineLoggerAdapter
{
	private static readonly StringBuilder _logBuilder = new();
	private static readonly Subject<string> _logSubject = new();

	public static IObservable<string> LogObservable => _logSubject;

	public static void AppendLine(string message)
	{
		_logBuilder.AppendLine(message);
		_logSubject.OnNext(_logBuilder.ToString());
	}

	public static void Clear()
	{
		_logBuilder.Clear();
		_logSubject.OnNext(string.Empty);
	}
}
