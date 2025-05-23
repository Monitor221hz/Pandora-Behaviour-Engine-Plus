using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Pandora.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace Pandora.Behaviors;

public sealed class ModsDataGridDropHandler : BaseDataGridDropHandler<ModInfoViewModel>
{
    protected override ModInfoViewModel MakeCopy(ObservableCollection<ModInfoViewModel> parentCollection, ModInfoViewModel dragItem) =>
        throw new NotImplementedException();

    protected override bool Validate(DataGrid dg, DragEventArgs e, object? sourceContext, object? targetContext, bool bExecute)
    {
        if (sourceContext is not ModInfoViewModel sourceItem
         || targetContext is not EngineViewModel vm
         || dg.GetVisualAt(e.GetPosition(dg)) is not Control targetControl
         || targetControl.DataContext is not ModInfoViewModel targetItem)
        {
            return false;
        }

        bool sourceIsPandora = string.Equals(sourceItem.Code, "pandora", StringComparison.OrdinalIgnoreCase);
        bool targetIsPandora = string.Equals(targetItem.Code, "pandora", StringComparison.OrdinalIgnoreCase);
        if (sourceIsPandora || targetIsPandora)
            return false;

        var items = vm.SourceMods;
        bool result = RunDropAction(dg, e, bExecute, sourceItem, targetItem, items);

        if (result && bExecute) 
            vm.ModService.AssignModPrioritiesFromViewModels(items);

        return result;
    }
}