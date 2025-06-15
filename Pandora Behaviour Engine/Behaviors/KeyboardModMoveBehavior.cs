using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using Pandora.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Behaviors;

public class KeyboardModMoveBehavior : StyledElementBehavior<DataGrid>
{
	public ModsDataGridDropHandler? DropHandler { get; set; }

	protected override void OnAttached()
	{
		base.OnAttached();
		AssociatedObject!.KeyDown += OnKeyDown;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		AssociatedObject!.KeyDown -= OnKeyDown;
	}

	private void OnKeyDown(object? sender, KeyEventArgs e)
	{
		if (AssociatedObject.DataContext is not EngineViewModel vm || DropHandler is null)
			return;

		bool isCtrl = (e.KeyModifiers & KeyModifiers.Control) == KeyModifiers.Control;

		if (e.Key == Key.Add || e.Key == Key.OemPlus || (isCtrl && e.Key == Key.Down))
		{
			DropHandler.MoveDown(AssociatedObject, vm);
			e.Handled = true;
		}
		else if (e.Key == Key.Subtract || e.Key == Key.OemMinus || (isCtrl && e.Key == Key.Up))
		{
			DropHandler.MoveUp(AssociatedObject, vm);
			e.Handled = true;
		}
	}
}
