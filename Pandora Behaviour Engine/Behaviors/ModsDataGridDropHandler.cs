using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Pandora.Utils;
using Pandora.ViewModels;
using System;
using System.Collections.ObjectModel;

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

		if (ModUtils.IsPandoraMod(sourceItem) || ModUtils.IsPandoraMod(targetItem))
			return false;

        var items = vm.SourceMods;

		return RunDropAction(dg, e, bExecute, sourceItem, targetItem, items);
    }

	public void MoveUp(DataGrid grid, EngineViewModel vm)
	{
		if (grid.SelectedItem is not ModInfoViewModel selected || ModUtils.IsPandoraMod(selected))
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
		if (grid.SelectedItem is not ModInfoViewModel selected || ModUtils.IsPandoraMod(selected))
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