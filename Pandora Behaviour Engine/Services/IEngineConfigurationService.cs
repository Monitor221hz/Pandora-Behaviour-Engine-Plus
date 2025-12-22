// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Collections.Generic;
using System.Reactive;
using Pandora.API.Patch.Config;
using Pandora.API.Patch.Engine.Config;
using Pandora.ViewModels;
using ReactiveUI;

namespace Pandora.Services;

public interface IEngineConfigurationService
{
	IEngineConfigurationFactory CurrentFactory { get; }

	IEnumerable<IEngineConfigurationViewModel> FlattenConfigurations(
		IEnumerable<IEngineConfigurationViewModel> configs
	);
	IReadOnlyCollection<IEngineConfigurationViewModel> GetConfigurations();
	IEngineConfigurationFactory? GetFactoryByType<T>()
		where T : IEngineConfiguration;
	void Initialize(
		bool useSkyrimDebug64,
		ReactiveCommand<IEngineConfigurationFactory, Unit> setCommand
	);
	void SetCurrentFactory(IEngineConfigurationFactory factory);
}
