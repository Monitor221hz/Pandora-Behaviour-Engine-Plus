// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia;
using Avalonia.Controls;
using Pandora.ViewModels;
using ReactiveUI.Avalonia;

namespace Pandora.Views;

public partial class EngineView : ReactiveUserControl<EngineViewModel>
{
	public EngineView()
	{
		InitializeComponent();
		this.PropertyChanged += OnPropertyChanged;
	}

	private void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
	{
		if (e.Property == BoundsProperty)
		{
			var bounds = (Rect)e.NewValue!;
			UpdateLayout(bounds.Width);
		}
	}

	private void UpdateLayout(double width)
	{
		bool isWide = width >= 1000;

		// Configure PatchBox
		Grid.SetRow(PatchBox, 0);
		Grid.SetRowSpan(PatchBox, isWide ? 3 : 1);
		Grid.SetColumn(PatchBox, 0);
		Grid.SetColumnSpan(PatchBox, isWide ? 1 : 3);
		PatchBox.Margin = isWide ? new Thickness(0, 6, 0, 10) : new Thickness(0, 6, 0, 0);

		// Configure MySplitter
		Grid.SetRow(MySplitter, isWide ? 0 : 1);
		Grid.SetRowSpan(MySplitter, isWide ? 3 : 1);
		Grid.SetColumn(MySplitter, isWide ? 1 : 0);
		Grid.SetColumnSpan(MySplitter, isWide ? 1 : 3);
		MySplitter.ResizeDirection = isWide
			? GridResizeDirection.Columns
			: GridResizeDirection.Rows;
		MySplitter.Width = isWide ? 8 : 90;
		MySplitter.Height = isWide ? 90 : 8;

		// Configure LogBox
		Grid.SetRow(LogBox, isWide ? 0 : 2);
		Grid.SetRowSpan(LogBox, isWide ? 3 : 1);
		Grid.SetColumn(LogBox, isWide ? 2 : 0);
		Grid.SetColumnSpan(LogBox, isWide ? 1 : 3);
		LogBox.Margin = isWide ? new Thickness(6, 6, 0, 10) : new Thickness(0, 6, 0, 10);
	}
}
