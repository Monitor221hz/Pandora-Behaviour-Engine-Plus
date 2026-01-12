// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Avalonia;
using Avalonia.Xaml.Interactivity;
using AvaloniaEdit;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Pandora.Behaviors;

public class LogTextEditorBehavior : Behavior<TextEditor>
{
	public static readonly StyledProperty<IObservable<string>?> LogStreamProperty =
		AvaloniaProperty.Register<LogTextEditorBehavior, IObservable<string>?>(nameof(LogStream));

	public static readonly StyledProperty<IObservable<Unit>?> ClearStreamProperty =
		AvaloniaProperty.Register<LogTextEditorBehavior, IObservable<Unit>?>(nameof(ClearStream));

	public IObservable<string>? LogStream
	{
		get => GetValue(LogStreamProperty);
		set => SetValue(LogStreamProperty, value);
	}

	public IObservable<Unit>? ClearStream
	{
		get => GetValue(ClearStreamProperty);
		set => SetValue(ClearStreamProperty, value);
	}

	private IDisposable? _logSubscription;
	private IDisposable? _clearSubscription;
	private const int MaxLines = 2000;

	protected override void OnAttached()
	{
		base.OnAttached();

		if (AssociatedObject is not null)
		{
			AssociatedObject.Options.AllowScrollBelowDocument = false;
			AssociatedObject.Options.EnableHyperlinks = false;
			AssociatedObject.Options.EnableEmailHyperlinks = false;
		}

		Subscribe();
	}

	protected override void OnDetaching()
	{
		_logSubscription?.Dispose();
		_clearSubscription?.Dispose();
		base.OnDetaching();
	}

	private void Subscribe()
	{
		_logSubscription?.Dispose();
		_clearSubscription?.Dispose();

		if (AssociatedObject is null) return;

		if (LogStream is not null)
		{
			_logSubscription = LogStream
				.ObserveOn(RxApp.MainThreadScheduler)
				.Subscribe(text => AppendText(text));
		}

		if (ClearStream is not null)
		{
			_clearSubscription = ClearStream
				.ObserveOn(RxApp.MainThreadScheduler)
				.Subscribe(_ => AssociatedObject.Document.Text = string.Empty);
		}
	}

	private void AppendText(string text)
	{
		if (AssociatedObject is null) return;

		var document = AssociatedObject.Document;

		document.BeginUpdate();
		document.Insert(document.TextLength, text);

		if (document.LineCount > MaxLines)
		{
			var linesToRemove = document.LineCount - MaxLines + 100;
			var lastLineToRemove = document.GetLineByNumber(linesToRemove);
			document.Remove(0, lastLineToRemove.Offset + lastLineToRemove.TotalLength);
		}
		document.EndUpdate();

		AssociatedObject.ScrollToEnd();
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		if (change.Property == LogStreamProperty || change.Property == ClearStreamProperty)
		{
			Subscribe();
		}
	}
}