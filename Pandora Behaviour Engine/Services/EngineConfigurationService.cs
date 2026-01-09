// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.DTOs;
using Pandora.API.Patch.Config;
using Pandora.API.Services;
using Pandora.Models.Patch.Configs;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace Pandora.Services;

public class EngineConfigurationService : IEngineConfigurationService
{
	private readonly IEngineConfigurationFactory<SkyrimConfiguration> _skyrimFactory;
	private readonly IEngineConfigurationFactory<SkyrimDebugConfiguration> _skyrimDebugFactory;

	private IEngineConfigurationFactory _currentFactory;
	private readonly List<EngineConfigDescriptor> _availableConfigs = [];

	private readonly BehaviorSubject<IEngineConfigurationFactory> _factorySubject;
	public IObservable<IEngineConfigurationFactory> CurrentFactoryChanged => _factorySubject;

	public IEngineConfigurationFactory CurrentFactory => _currentFactory;

	public EngineConfigurationService(
		IEngineConfigurationFactory<SkyrimConfiguration> skyrimFactory,
		IEngineConfigurationFactory<SkyrimDebugConfiguration> skyrimDebugFactory)
	{
		_skyrimFactory = skyrimFactory;
		_skyrimDebugFactory = skyrimDebugFactory;
		_currentFactory = skyrimFactory;
		_factorySubject = new BehaviorSubject<IEngineConfigurationFactory>(_currentFactory);
	}

	public void Initialize(bool useSkyrimDebug64)
	{
		_availableConfigs.Clear();

		_availableConfigs.Add(new EngineConfigDescriptor(_skyrimFactory, "Lean", "Skyrim 64/Behavior/Patch"));
		_availableConfigs.Add(new EngineConfigDescriptor(_skyrimDebugFactory, "Include Debug", "Skyrim 64/Behavior/Patch"));

		if (useSkyrimDebug64)
		{
			SetCurrentFactory(_skyrimDebugFactory);
		}
	}

	public void SetCurrentFactory(IEngineConfigurationFactory factory)
	{
		if (factory != null && _currentFactory != factory)
		{
			_currentFactory = factory;
			_factorySubject.OnNext(factory);
		}
	}

	public IReadOnlyCollection<EngineConfigDescriptor> GetAvailableConfigurations() => _availableConfigs;
}