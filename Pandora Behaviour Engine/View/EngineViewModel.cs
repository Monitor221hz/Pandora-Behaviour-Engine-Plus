// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

namespace Pandora.ViewModels;

public partial class EngineViewModel : ViewModelBase
{
	public IEngineSharedState State { get; }

	public PatchBoxViewModel PatchBoxVM { get; }
	public LogBoxViewModel LogBoxVM { get; }
	public LaunchElementViewModel LaunchElementVM { get; }

	public EngineViewModel(
		IEngineSharedState state,
		PatchBoxViewModel patchBox,
		LogBoxViewModel log,
		LaunchElementViewModel launcher)
	{
		State = state;

		PatchBoxVM = patchBox;
		LogBoxVM = log;
		LaunchElementVM = launcher;
	}
}
