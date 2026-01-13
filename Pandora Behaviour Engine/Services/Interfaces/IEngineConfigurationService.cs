// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Config;
using Pandora.DTOs;
using System;
using System.Collections.Generic;

namespace Pandora.Services.Interfaces;

public interface IEngineConfigurationService
{
    IObservable<IEngineConfigurationFactory> CurrentFactoryChanged { get; }
    IEngineConfigurationFactory CurrentFactory { get; }

	void Initialize(bool useSkyrimDebug64);

	void SetCurrentFactory(IEngineConfigurationFactory factory);

	IReadOnlyCollection<EngineConfigDescriptor> GetAvailableConfigurations();
}