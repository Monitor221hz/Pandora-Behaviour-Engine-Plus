// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactions.DragAndDrop;
using Pandora.Utils;
using Pandora.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace Pandora.Behaviors;

public sealed class ModsDataGridDropHandler : BaseDataGridDropHandler<ModInfoViewModel>
{
	protected override ModInfoViewModel MakeCopy(ObservableCollection<ModInfoViewModel> parentCollection, ModInfoViewModel item)
		=> throw new System.NotImplementedException();

	protected override bool Validate(DataGrid dg, DragEventArgs e, object? sourceContext, object? targetContext, bool execute)
	{
		if (sourceContext is not ModInfoViewModel sourceItem
		 || targetContext is not PatchBoxViewModel vm
		 || dg.GetVisualAt(e.GetPosition(dg)) is not Control targetControl
		 || targetControl.DataContext is not ModInfoViewModel targetItem)
		{
			return false;
		}

		if (sourceItem.IsPandora)
			return false;

		if (!execute)
			return true;

		var proxyCollection = new ObservableCollection<ModInfoViewModel>(vm.ModViewModels);

		bool result = RunDropAction(dg, e, true, sourceItem, targetItem, proxyCollection);

		if (result)
		{
			var lastItem = vm.ModViewModels.Last();
			if (proxyCollection.Last() != lastItem)
			{
				return false;
			}

			proxyCollection.RecalculatePriorities();
		}

		return result;
	}
}