// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

namespace Pandora.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
	public EngineViewModel EngineVM { get; }
	public EngineMenuViewModel EngineMenuVM { get; }

	public MainWindowViewModel(EngineViewModel viewModel, EngineMenuViewModel engineMenuVM)
	{
		EngineVM = viewModel;
		EngineMenuVM = engineMenuVM;
	}
}
