// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Avalonia;
using Avalonia.Xaml.Interactivity;
using AvaloniaEdit;
using Pandora.Logging.NLogger.UI;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace Pandora.Behaviors;

public class LogTextEditorBehavior : Behavior<TextEditor>
{
	public static readonly StyledProperty<IObservable<LogUiEvent>?> LogSourceProperty =
		AvaloniaProperty.Register<LogTextEditorBehavior, IObservable<LogUiEvent>?>(nameof(LogSource));

	public IObservable<LogUiEvent>? LogSource
	{
		get => GetValue(LogSourceProperty);
		set => SetValue(LogSourceProperty, value);
	}

	private IDisposable? _subscription;
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
		_subscription?.Dispose();
		base.OnDetaching();
	}

	private void Subscribe()
	{
		_subscription?.Dispose();

		if (AssociatedObject is null || LogSource is null) return;

		_subscription = LogSource
			.ObserveOn(RxApp.MainThreadScheduler)
			.Subscribe(evt =>
			{
				switch (evt)
				{
					case LogUiEvent.Message msg:
						AppendText(msg.Text);
						break;
					case LogUiEvent.Clear:
						AssociatedObject.Document.Text = string.Empty;
						break;
				}
			});
	}

	private void AppendText(string text)
	{
		if (AssociatedObject is null) return;

		var document = AssociatedObject.Document;
		document.BeginUpdate();

		document.Insert(document.TextLength, text + Environment.NewLine);

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
		if (change.Property == LogSourceProperty)
		{
			Subscribe();
		}
	}
}