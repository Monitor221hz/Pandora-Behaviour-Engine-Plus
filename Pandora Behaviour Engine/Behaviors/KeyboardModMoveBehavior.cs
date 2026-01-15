// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using Pandora.Mods.Extensions;
using Pandora.ViewModels;

namespace Pandora.Behaviors;

public sealed class KeyboardModMoveBehavior : Behavior<DataGrid>
{
	protected override void OnAttached()
	{
		base.OnAttached();
		AssociatedObject?.KeyDown += OnKeyDown;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		AssociatedObject?.KeyDown -= OnKeyDown;
	}

	private void OnKeyDown(object? sender, KeyEventArgs e)
	{
		int direction = 0;
		if (e.Key == Key.OemMinus || e.Key == Key.Subtract) direction = -1;
		else if (e.Key == Key.OemPlus || e.Key == Key.Add) direction = 1;

		if (direction == 0) return;

		var dataGrid = AssociatedObject;

		if (dataGrid?.DataContext is not PatchBoxViewModel vm ||
			dataGrid.SelectedItem is not ModInfoViewModel selectedMod)
			return;

		bool moved = vm.ModViewModels.TryMoveAndRecalculate(selectedMod, direction);

		if (moved)
		{
			e.Handled = true;

			dataGrid.SelectedItem = selectedMod;
			dataGrid.ScrollIntoView(selectedMod, null);
		}
	}
}