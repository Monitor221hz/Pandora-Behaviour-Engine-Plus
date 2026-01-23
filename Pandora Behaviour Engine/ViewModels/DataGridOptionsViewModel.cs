// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Avalonia.Controls;
using Pandora.Utils;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Pandora.ViewModels;

public partial class DataGridOptionsViewModel : ViewModelBase
{
	[Reactive]
	private bool _isCompactRowHeight;

	[Reactive]
	private DataGridGridLinesVisibility _gridLinesVisibility;

	public DataGridOptionsViewModel()
	{
		InitProperties();
		InitSubscriptions();
	}

	private void InitProperties()
	{
		IsCompactRowHeight = Properties.GUISettings.Default.IsCompactRowHeight;
		GridLinesVisibility = (DataGridGridLinesVisibility)
			Properties.GUISettings.Default.GridLinesVisibility;
	}

	private void InitSubscriptions()
	{
		ViewModelSettingsHelper.BindSetting(
			this.WhenAnyValue(vm => vm.IsCompactRowHeight),
			val => Properties.GUISettings.Default.IsCompactRowHeight = val
		);

		ViewModelSettingsHelper.BindSetting(
			this.WhenAnyValue(vm => vm.GridLinesVisibility),
			val => Properties.GUISettings.Default.GridLinesVisibility = (int)val
		);
	}
}
