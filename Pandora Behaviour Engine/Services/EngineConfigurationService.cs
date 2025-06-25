using Pandora.API.Patch.Engine.Config;
using Pandora.Models;
using Pandora.Models.Patch.Configs;
using Pandora.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace Pandora.Services;

public class EngineConfigurationService
{
	private readonly ObservableCollection<IEngineConfigurationViewModel> _configurations = [];
	private readonly char[] _pathSeparators = ['/', '\\'];

	private IEngineConfigurationFactory _currentFactory = new ConstEngineConfigurationFactory<SkyrimConfiguration>("Normal");

	public void SetCurrentFactory(IEngineConfigurationFactory factory) => _currentFactory = factory;
	public IEngineConfigurationFactory GetCurrentFactory() => _currentFactory;

	public IReadOnlyCollection<IEngineConfigurationViewModel> GetInitialConfigurations(ReactiveCommand<IEngineConfigurationFactory, Unit> setCommand)
	{
		_configurations.Clear();

		var root = new EngineConfigurationViewModelContainer("Skyrim 64",
			new EngineConfigurationViewModelContainer("Behavior",
				new EngineConfigurationViewModelContainer("Patch",
					new EngineConfigurationViewModel(_currentFactory, setCommand),
					new EngineConfigurationViewModel(new ConstEngineConfigurationFactory<SkyrimDebugConfiguration>("Debug"), setCommand)
				)
			)
		);

		_configurations.Add(root);

		foreach (var plugin in BehaviourEngine.EngineConfigurations)
		{
			RegisterPlugin(plugin, setCommand);
		}

		return _configurations;
	}

	private void RegisterPlugin(IEngineConfigurationPlugin plugin, ReactiveCommand<IEngineConfigurationFactory, Unit> setCommand)
	{
		if (string.IsNullOrWhiteSpace(plugin.MenuPath))
		{
			_configurations.Add(new EngineConfigurationViewModel(plugin.Factory, setCommand));
			return;
		}

		var pathSegments = plugin.MenuPath.Split(_pathSeparators, StringSplitOptions.RemoveEmptyEntries);

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

		currentContainer?.NestedViewModels.Add(new EngineConfigurationViewModel(plugin.Factory, setCommand));
	}

	public IEngineConfigurationFactory? GetFactoryByType<T>() where T : IEngineConfiguration
	{
		return FlattenConfigurations(_configurations)
			.OfType<EngineConfigurationViewModel>()
			.Select(vm => vm.Factory)
			.FirstOrDefault(factory => factory.Config is T);
	}

	public static IEnumerable<IEngineConfigurationViewModel> FlattenConfigurations(IEnumerable<IEngineConfigurationViewModel> configs)
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
