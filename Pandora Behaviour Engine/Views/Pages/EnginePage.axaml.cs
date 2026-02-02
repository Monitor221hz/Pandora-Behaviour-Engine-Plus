// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Pandora.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pandora.Views;

public partial class EnginePage : ReactiveUserControl<EnginePageViewModel>
{
	private double _lastLogBoxHeight = 360;
	private CancellationTokenSource? _animationCts;
	private bool? _wasWideLayout = null;

	public EnginePage()
	{
		InitializeComponent();

		this.WhenActivated(disposables =>
		{
			this.WhenAnyValue(x => x.Bounds, x => x.LogBox.LogExpander.IsExpanded)
				.ObserveOn(RxApp.MainThreadScheduler)
				.Subscribe(x => UpdateGridLayout(x.Item1.Width, x.Item2))
				.DisposeWith(disposables);

			Observable.FromEventPattern<RoutedEventArgs>(
					h => LogBox.LogExpander.Expanded += h,
					h => LogBox.LogExpander.Expanded -= h)
				.Select(_ => true)
				.Merge(
					Observable.FromEventPattern<RoutedEventArgs>(
						h => LogBox.LogExpander.Collapsed += h,
						h => LogBox.LogExpander.Collapsed -= h)
					.Select(_ => false)
				)
				.Subscribe(async isExpanded => await AnimateLogBox(isExpanded))
				.DisposeWith(disposables);

			Observable.FromEventPattern<VectorEventArgs>(
					h => MySplitter.DragCompleted += h,
					h => MySplitter.DragCompleted -= h)
				.Subscribe(_ =>
				{
					var h = RootGrid.RowDefinitions[3].ActualHeight;
					if (h > 50) _lastLogBoxHeight = h;
				})
				.DisposeWith(disposables);
		});
	}

	private async Task AnimateLogBox(bool isExpanded)
	{
		if (_animationCts != null)
		{
			_animationCts.Cancel();
			_animationCts.Dispose();
		}
		_animationCts = new CancellationTokenSource();
		var token = _animationCts.Token;

		var rowDef = RootGrid.RowDefinitions[3];

		double startHeight = rowDef.ActualHeight;
		double targetHeight = isExpanded ? Math.Max(_lastLogBoxHeight, 128) : 54;

		if (Math.Abs(startHeight - targetHeight) < 1) return;

		if (isExpanded)
		{
			MySplitter.IsVisible = true;
			rowDef.MinHeight = 128;
		}
		else
		{
			rowDef.MinHeight = 0;
		}

		try
		{
			var heightAnim = (Animation)this.Resources["RowHeightAnimation"]!;

			if (heightAnim.Children[0].Setters[0] is Setter startSetter)
				startSetter.Value = startHeight;

			if (heightAnim.Children[1].Setters[0] is Setter endSetter)
				endSetter.Value = targetHeight;

			var animatableRow = new RowHeightAnimatable(rowDef);
			var heightTask = heightAnim.RunAsync(animatableRow, token);

			var splitterAnimKey = isExpanded ? "SplitterFadeIn" : "SplitterFadeOut";
			var splitterAnim = (Animation)this.Resources[splitterAnimKey]!;

			var opacityTask = splitterAnim.RunAsync(MySplitter, token);

			await Task.WhenAll(heightTask, opacityTask);
		}
		catch (OperationCanceledException)
		{

		}
		finally
		{
			if (!isExpanded && !token.IsCancellationRequested)
			{
				MySplitter.IsVisible = false;
			}

			UpdateGridLayout(Bounds.Width, isExpanded);
		}
	}

	private void UpdateGridLayout(double width, bool isExpanded)
	{
		bool isWide = width >= 1000;

		if (!isExpanded && isWide)
			isWide = false;

		if (_wasWideLayout == isWide) return;
		_wasWideLayout = isWide;

		static void Place(Control ctrl, int r, int rs, int c, int cs, Thickness margin)
		{
			Grid.SetRow(ctrl, r);
			Grid.SetRowSpan(ctrl, rs);
			Grid.SetColumn(ctrl, c);
			Grid.SetColumnSpan(ctrl, cs);
			ctrl.Margin = margin;
		}

		if (isWide)
		{
			Place(PatchBox, 1, 3, 0, 1, new Thickness(0, 0, 0, 0));

			Place(MySplitter, 1, 3, 1, 1, MySplitter.Margin);
			MySplitter.ResizeDirection = GridResizeDirection.Columns;
			MySplitter.Width = 8;
			MySplitter.Height = 100;

			Place(LogBox, 1, 3, 2, 1, new Thickness(0, 0, 0, 0));
		}
		else
		{
			Place(PatchBox, 1, 1, 0, 3, new Thickness(0, 0, 0, 0));

			Place(MySplitter, 2, 1, 0, 3, MySplitter.Margin);
			MySplitter.ResizeDirection = GridResizeDirection.Rows;
			MySplitter.Width = 100;
			MySplitter.Height = 8;

			Place(LogBox, 3, 1, 0, 3, new Thickness(0, 0, 0, 0));
		}
	}
}

public class RowHeightAnimatable(RowDefinition row) : Animatable
{
	public static readonly DirectProperty<RowHeightAnimatable, double> HeightProperty =
		AvaloniaProperty.RegisterDirect<RowHeightAnimatable, double>(
			nameof(Height),
			o => o.Height,
			(o, v) => o.Height = v
		);

	public double Height
	{
		get => row.Height.Value;
		set => row.Height = new GridLength(value, GridUnitType.Pixel);
	}
}