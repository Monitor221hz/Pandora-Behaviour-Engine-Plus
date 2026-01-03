// Pandora/Behaviors/KeyboardModMoveBehavior.cs
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
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
		var dataGrid = AssociatedObject;
		if (dataGrid?.DataContext is not EngineViewModel vm ||
			dataGrid.SelectedItem is not ModInfoViewModel selectedMod)
			return;

		bool handled = false;

		if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
		{
			vm.MoveModStep(selectedMod, -1);
			handled = true;
		}
		else if (e.Key == Key.OemPlus || e.Key == Key.Add)
		{
			vm.MoveModStep(selectedMod, 1);
			handled = true;
		}

		if (handled)
		{
			e.Handled = true;

			dataGrid.SelectedItem = selectedMod;
			dataGrid.ScrollIntoView(selectedMod, null);
		}
	}
}