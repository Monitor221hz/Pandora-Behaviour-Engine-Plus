// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Config;
using System;
using System.Collections.Generic;

namespace Pandora.Configuration;

public interface IEngineConfigurationService
{
    IObservable<IEngineConfigurationFactory> CurrentFactoryChanged { get; }
    IEngineConfigurationFactory CurrentFactory { get; }

	void Initialize(bool useSkyrimDebug64);

	void SetCurrentFactory(IEngineConfigurationFactory factory);

	void RegisterConfiguration(IEngineConfigurationFactory factory, string displayName, string menuPath);

	IReadOnlyCollection<EngineConfigDescriptor> GetAvailableConfigurations();
}