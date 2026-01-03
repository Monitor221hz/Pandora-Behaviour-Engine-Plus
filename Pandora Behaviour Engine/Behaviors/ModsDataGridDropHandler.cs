// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactions.DragAndDrop;
using Pandora.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace Pandora.Behaviors;

public sealed class ModsDataGridDropHandler : BaseDataGridDropHandler<ModInfoViewModel>
{
	protected override ModInfoViewModel MakeCopy(ObservableCollection<ModInfoViewModel> parentCollection, ModInfoViewModel dragItem) => throw new NotImplementedException();

	protected override bool Validate(DataGrid dg, DragEventArgs e, object? sourceContext, object? targetContext, bool execute)
	{
		if (sourceContext is not ModInfoViewModel sourceItem
			|| targetContext is not EngineViewModel vm
			|| dg.GetVisualAt(e.GetPosition(dg)) is not Control targetControl
			|| targetControl.DataContext is not ModInfoViewModel targetItem)
		{
			return false;
		}

		if (!execute) return true;

		vm.ReorderMod(sourceItem, targetItem);

		return true;
	}
}