// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Platform.Avalonia;
using Pandora.Platform.CreationEngine;
using Pandora.Settings;
using Pandora.Views.Pages;
using Pandora.Views.Pages.DTOs;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Pandora.ViewModels;

public partial class SettingsPageViewModel : RoutableViewModelBase, IActivatableViewModel
{
	public override string UrlPathSegment => Routes.Settings;
	public ViewModelActivator Activator { get; } = new();

	private readonly ISettingsService _settings;
	private readonly IDiskDialogService _diskDialog;
	private readonly IGameDescriptor _gameDescriptor;

	public IReadOnlyList<AppTheme> Themes { get; } = Enum.GetValues<AppTheme>();
	public AppTheme SelectedTheme
	{
		get => _settings.Theme.Theme;
		set => _settings.Theme.Theme = value;
	}

	[ObservableAsProperty]
	private string _gameDataPath = string.Empty;

	[ObservableAsProperty]
	private string _outputPath = string.Empty;

	public SettingsPageViewModel(
		ISettingsService settings,
		IDiskDialogService diskDialog,
		IGameDescriptor gameDescriptor,
		IScreen screen) : base(screen)
	{
		_settings = settings;
		_diskDialog = diskDialog;
		_gameDescriptor = gameDescriptor;

		_gameDataPathHelper = _settings.Paths.GameDataChanged
			.Select(d => d.FullName)
			.ToProperty(this, x => x.GameDataPath);

		_outputPathHelper = _settings.Paths.OutputChanged
			.Select(d => d.FullName)
			.ToProperty(this, x => x.OutputPath);

		this.WhenActivated(disposables =>
		{
			if (_settings.Paths.NeedsUserSelection)
			{
				Observable.StartAsync(PickGameDirectory)
					.Subscribe()
					.DisposeWith(disposables);
			}
		});
	}

	[ReactiveCommand]
	private async Task PickGameDirectory()
	{
		var file = await _diskDialog.OpenFileAsync(
			$"Select {_gameDescriptor.Name} Executable",
			_settings.Paths.GameData,
			["*.exe"]);

		if (file != null && file.Directory != null)
		{
			_settings.Paths.SetGameDataFolder(file.Directory);
		}
	}

	[ReactiveCommand]
	private async Task PickOutputDirectory()
	{
		var folder = await _diskDialog.OpenFolderAsync(
			"Select Output Directory",
			_settings.Paths.Output);

		if (folder != null && folder.Exists)
		{
			_settings.Paths.SetOutputFolder(folder);
		}
	}
}