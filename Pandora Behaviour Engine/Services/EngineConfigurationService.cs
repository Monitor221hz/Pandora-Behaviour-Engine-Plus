// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Pandora.API.Patch.Config;
using Pandora.API.Patch.Engine.Config;
using Pandora.Models;
using Pandora.Models.Patch.Configs;
using Pandora.ViewModels;
using Pandora.ViewModels.Configuration;
using ReactiveUI;

namespace Pandora.Services;

public class EngineConfigurationService(
	IEngineConfigurationFactory<SkyrimConfiguration> skyrimFactory,
	IEngineConfigurationFactory<SkyrimDebugConfiguration> skyrimDebugFactory
) : IEngineConfigurationService
{
	private readonly IEngineConfigurationFactory<SkyrimConfiguration> _skyrimFactory =
		skyrimFactory;
	private readonly IEngineConfigurationFactory<SkyrimDebugConfiguration> _skyrimDebugFactory =
		skyrimDebugFactory;

	private readonly ObservableCollection<IEngineConfigurationViewModel> _configurations = [];
	private readonly char[] _pathSeparators = ['/', '\\'];

	private IEngineConfigurationFactory _currentFactory = skyrimFactory;
	public IEngineConfigurationFactory CurrentFactory => _currentFactory;

	public void SetCurrentFactory(IEngineConfigurationFactory factory) => _currentFactory = factory;

	public void Initialize(
		bool useSkyrimDebug64,
		ReactiveCommand<IEngineConfigurationFactory, Unit> setCommand
	)
	{
		if (useSkyrimDebug64)
			_currentFactory = _skyrimDebugFactory;

		_configurations.Clear();
		var root = new EngineConfigurationViewModelContainer(
			"Skyrim 64",
			new EngineConfigurationViewModelContainer(
				"Behavior",
				new EngineConfigurationViewModelContainer(
					"Patch",
					new EngineConfigurationViewModel(_skyrimFactory, "Lean", setCommand),
					new EngineConfigurationViewModel(
						_skyrimDebugFactory,
						"Include Debug",
						setCommand
					)
				)
			)
		);
		_configurations.Add(root);

		foreach (var plugin in BehaviourEngine.EngineConfigurations)
		{
			RegisterPlugin(plugin, setCommand);
		}
	}

	public IReadOnlyCollection<IEngineConfigurationViewModel> GetConfigurations() =>
		_configurations;

	private void RegisterPlugin(
		IEngineConfigurationPlugin plugin,
		ReactiveCommand<IEngineConfigurationFactory, Unit> setCommand
	)
	{
		if (string.IsNullOrWhiteSpace(plugin.MenuPath))
		{
			_configurations.Add(
				new EngineConfigurationViewModel(plugin.Factory, plugin.DisplayName, setCommand)
			);
			return;
		}

		var pathSegments = plugin.MenuPath.Split(
			_pathSeparators,
			StringSplitOptions.RemoveEmptyEntries
		);

		EngineConfigurationViewModelContainer? currentContainer = null;
		foreach (var segment in pathSegments)
		{
			var children = currentContainer?.NestedViewModels ?? _configurations;
			var existing = children
				.OfType<EngineConfigurationViewModelContainer>()
				.FirstOrDefault(c => c.Name.Equals(segment, StringComparison.OrdinalIgnoreCase));

			if (existing is null)
			{
				existing = new EngineConfigurationViewModelContainer(segment);
				children.Add(existing);
			}

			currentContainer = existing;
		}

		currentContainer?.NestedViewModels.Add(
			new EngineConfigurationViewModel(plugin.Factory, plugin.DisplayName, setCommand)
		);
	}

	public IEngineConfigurationFactory? GetFactoryByType<T>()
		where T : IEngineConfiguration
	{
		return FlattenConfigurations(_configurations)
			.OfType<EngineConfigurationViewModel>()
			.Select(vm => vm.Factory)
			.FirstOrDefault(factory => factory.Create() is T);
	}

	public IEnumerable<IEngineConfigurationViewModel> FlattenConfigurations(
		IEnumerable<IEngineConfigurationViewModel> configs
	)
	{
		foreach (var config in configs)
		{
			yield return config;

			if (config is EngineConfigurationViewModelContainer container)
			{
				foreach (var nested in FlattenConfigurations(container.NestedViewModels))
				{
					yield return nested;
				}
			}
		}
	}
}
