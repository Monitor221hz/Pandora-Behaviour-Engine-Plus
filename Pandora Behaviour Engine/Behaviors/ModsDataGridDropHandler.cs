using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Styling;
using Avalonia.VisualTree;
using Pandora.Services;
using Pandora.Utils;
using Pandora.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Pandora.Behaviors;

public sealed class ModsDataGridDropHandler : BaseDataGridDropHandler<ModInfoViewModel>
{
    protected override ModInfoViewModel MakeCopy(ObservableCollection<ModInfoViewModel> parentCollection, ModInfoViewModel dragItem) =>
        throw new NotImplementedException();

	public ModsDataGridDropHandler()
		: base((mod, priority) => mod.Priority = priority) { }

	protected override bool Validate(DataGrid dg, DragEventArgs e, object? sourceContext, object? targetContext, bool bExecute)
    {
        if (sourceContext is not ModInfoViewModel sourceItem
         || targetContext is not EngineViewModel vm
         || dg.GetVisualAt(e.GetPosition(dg)) is not Control targetControl
         || targetControl.DataContext is not ModInfoViewModel targetItem)
        {
            return false;
        }

		if (ModUtils.IsPandora(sourceItem) || ModUtils.IsPandora(targetItem))
			return false;

        var items = vm.SourceMods;
		int targetIndex = items.IndexOf(targetItem);

		if (targetIndex == items.Count - 1)
			return false;

		return RunDropAction(dg, e, bExecute, sourceItem, targetItem, items);
    }

	public void MoveUp(DataGrid grid, EngineViewModel vm)
	{
		if (grid.SelectedItem is not ModInfoViewModel selected || ModUtils.IsPandora(selected))
			return;

		var items = vm.SourceMods;
		int index = items.IndexOf(selected);
		if (index > 0)
		{
			MoveItem(items, index, index - 1);
			AssignPriorities(items);
			grid.SelectedIndex = index - 1;
		}
	}

	public void MoveDown(DataGrid grid, EngineViewModel vm)
	{
		if (grid.SelectedItem is not ModInfoViewModel selected || ModUtils.IsPandora(selected))
			return;

		var items = vm.SourceMods;
		int index = items.IndexOf(selected);

		if (index < 0 || index >= items.Count - 2) // -2, Pandora base latest
			return;

		MoveItem(items, index, index + 1);
		AssignPriorities(items);
		grid.SelectedIndex = index + 1;
	}
}