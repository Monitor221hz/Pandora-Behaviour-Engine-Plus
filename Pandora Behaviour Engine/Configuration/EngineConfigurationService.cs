// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Config;
using Pandora.Logging.Extensions;
using Pandora.Models.Patch.Configs;
using Pandora.Models.Patch.Plugins;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace Pandora.Configuration;

public sealed class EngineConfigurationService : IEngineConfigurationService
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private readonly IPluginManager _pluginManager;
	private readonly IEngineConfigurationFactory<SkyrimConfiguration> _skyrimFactory;
	private readonly IEngineConfigurationFactory<SkyrimDebugConfiguration> _skyrimDebugFactory;

	private IEngineConfigurationFactory _currentFactory;
	private readonly List<EngineConfigDescriptor> _availableConfigs = [];

	private readonly BehaviorSubject<IEngineConfigurationFactory> _factorySubject;
	public IObservable<IEngineConfigurationFactory> CurrentFactoryChanged => _factorySubject;

	public IEngineConfigurationFactory CurrentFactory => _currentFactory;

	public EngineConfigurationService(
		IPluginManager pluginManager,
		IEngineConfigurationFactory<SkyrimConfiguration> skyrimFactory,
		IEngineConfigurationFactory<SkyrimDebugConfiguration> skyrimDebugFactory)
	{
		_pluginManager = pluginManager;
		_skyrimFactory = skyrimFactory;
		_skyrimDebugFactory = skyrimDebugFactory;
		_currentFactory = skyrimFactory;
		_factorySubject = new BehaviorSubject<IEngineConfigurationFactory>(_currentFactory);
	}

	public void Initialize(bool useSkyrimDebug64)
	{
		_availableConfigs.Clear();

		RegisterConfiguration(_skyrimFactory, "Lean");
		RegisterConfiguration(_skyrimDebugFactory, "Include Debug");

		foreach (var plugin in _pluginManager.EngineConfigurationPlugins)
		{
			RegisterConfiguration(plugin.Factory, plugin.DisplayName, plugin.MenuPath);
		}

		if (_pluginManager.EngineConfigurationPlugins.Count > 0)
		{
			logger.UiInfo("Plugins loaded.");
		}

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

	public void RegisterConfiguration(
	IEngineConfigurationFactory factory,
	string displayName,
	string? menuPath)
	{
		_availableConfigs.Add(
			new EngineConfigDescriptor(factory, displayName, menuPath));
	}

	public void RegisterConfiguration(
	IEngineConfigurationFactory factory,
	string displayName)
	{
		_availableConfigs.Add(
			new EngineConfigDescriptor(factory, displayName));
	}


	public IReadOnlyCollection<EngineConfigDescriptor> GetAvailableConfigurations() => _availableConfigs;
}