// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Configuration;
using Pandora.Configuration.ViewModels;
using System.Collections.ObjectModel;

namespace Pandora.ViewModels;

public partial class EngineMenuViewModel(
		IEngineSharedState state,
		IEngineConfigurationService engineConfigService) : ViewModelBase
{
	public IEngineSharedState State { get; } = state;

	public ObservableCollection<IEngineConfigurationViewModel> EngineConfigurationViewModels { get; } = 
		ConfigurationMenuBuilder.BuildTree(
			engineConfigService.GetAvailableConfigurations(),
			engineConfigService
		);
}
