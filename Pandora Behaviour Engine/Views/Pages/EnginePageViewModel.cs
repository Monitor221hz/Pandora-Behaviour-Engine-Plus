// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Configuration;
using Pandora.Configuration.ViewModels;
using Pandora.Views.Pages;
using Pandora.Views.Pages.DTOs;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace Pandora.ViewModels;

public partial class EnginePageViewModel : RoutableViewModelBase
{
	public override string UrlPathSegment => Routes.Engine;

	public IEngineSharedState State { get; }

	public PatchBoxViewModel PatchBoxVM { get; }
	public LogBoxViewModel LogBoxVM { get; }

	public ObservableCollection<IEngineConfigurationViewModel> EngineConfigurationViewModels { get; } = [];

	public EnginePageViewModel(
		IEngineSharedState state,
		IEngineConfigurationService engineConfigService,
		PatchBoxViewModel patchBox,
		LogBoxViewModel log,
		IScreen screen) : base(screen)
	{
		State = state;

		PatchBoxVM = patchBox;
		LogBoxVM = log;

		EngineConfigurationViewModels = ConfigurationMenuBuilder.BuildTree(
			engineConfigService.GetAvailableConfigurations(),
			engineConfigService
		);
	}
}
