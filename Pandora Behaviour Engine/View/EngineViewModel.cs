// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.DTOs;

namespace Pandora.ViewModels;

public partial class EngineViewModel : ViewModelBase
{
	private readonly IEngineSessionState _state;
	public IEngineSessionState State => _state;

	public PatchBoxViewModel PatchBoxVM { get; }
	public LogBoxViewModel LogVM { get; }
	public LaunchElementViewModel LauncherVM { get; }

	public EngineViewModel(
		IEngineSessionState state,
		PatchBoxViewModel patchBox,
		LogBoxViewModel log,
		LaunchElementViewModel launcher)
	{
		_state = state;

		PatchBoxVM = patchBox;
		LogVM = log;
		LauncherVM = launcher;
	}
}
