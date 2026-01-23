// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Config;
using Pandora.ViewModels;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace Pandora.Configuration.ViewModels;

public partial class ConfigurationOptionViewModel : ViewModelBase, IEngineConfigurationViewModel, IDisposable
{
	private readonly IEngineConfigurationService _configService;

	private readonly CompositeDisposable _disposables = [];

	public string Name { get; }
	public IEngineConfigurationFactory Factory { get; }
	public IReactiveCommand? SelectCommand => ExecuteSelectCommand;

	[ObservableAsProperty]
	private bool _isChecked;

	public ObservableCollection<IEngineConfigurationViewModel> Children { get; } = [];

	public ConfigurationOptionViewModel(
		string name,
		IEngineConfigurationFactory factory,
		IEngineConfigurationService configService)
	{
		Name = name;
		Factory = factory;
		_configService = configService;

		_isCheckedHelper = _configService.CurrentFactoryChanged
			.Select(current => current == Factory)
			.ObserveOn(RxApp.MainThreadScheduler)
			.ToProperty(this, x => x.IsChecked)
			.DisposeWith(_disposables);
	}

	[ReactiveCommand]
	public void ExecuteSelect() => _configService.SetCurrentFactory(Factory);

	public void Dispose()
	{
		_disposables.Dispose();
		GC.SuppressFinalize(this);
	}
}