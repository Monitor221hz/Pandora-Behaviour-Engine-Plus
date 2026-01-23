using Pandora.Configuration;
using Pandora.Configuration.ViewModels;
using Pandora.Paths.Abstractions;
using Pandora.Platform.Avalonia;
using Pandora.Platform.CreationEngine;
using Pandora.Settings;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Pandora.ViewModels;

public partial class EngineMenuViewModel : ViewModelBase, IActivatableViewModel
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private readonly ISettingsService _settings;
	private readonly IUserPaths _userPaths;

	private readonly IEngineConfigurationService _engineConfigService;
	private readonly IDiskDialogService _diskDialogService;
	private readonly IGameDescriptor _gameDescriptor;

	public IEngineSharedState State { get; }

	public AboutDialogViewModel AboutDialog { get; }
	public SettingsViewModel Settings { get; }

	public Interaction<AboutDialogViewModel, Unit> ShowAboutDialog { get; } = new();

	public ViewModelActivator Activator { get; } = new();

	public ObservableCollection<IEngineConfigurationViewModel> EngineConfigurationViewModels { get; } = [];

	public EngineMenuViewModel(
		ISettingsService settings,
		IUserPaths userPaths,
		IEngineSharedState state,
		IEngineConfigurationService engineConfigService,
		IDiskDialogService diskDialogService,
		IGameDescriptor gameDescriptor,
		SettingsViewModel settingsViewModel,
		AboutDialogViewModel AboutDialogViewModel)
	{
		State = state;

		_settings = settings;
		_userPaths = userPaths;

		_engineConfigService = engineConfigService;
		_diskDialogService = diskDialogService;
		_gameDescriptor = gameDescriptor;

		AboutDialog = AboutDialogViewModel;
		Settings = settingsViewModel;

		EngineConfigurationViewModels = ConfigurationMenuBuilder.BuildTree(
			_engineConfigService.GetAvailableConfigurations(),
			_engineConfigService
		);

		this.WhenActivated(disposables =>
		{
			_settings
				.WhenAnyValue(x => x.NeedsUserSelection)
				.Where(x => x)
				.Take(1)
				.ObserveOn(RxApp.MainThreadScheduler)
				.Subscribe(async _ => await PickGameDirectory())
				.DisposeWith(disposables);
		});

	}

	[ReactiveCommand]
	private async Task PickGameDirectory()
	{
		var file = await _diskDialogService.OpenFileAsync(
			$"Select {_gameDescriptor.Name} Executable",
			_userPaths.GameData,
			["*.exe"]);

		if (file is null)
			return;

		_settings.SetGameDataFolder(file.Directory!);
	}

	[ReactiveCommand]
	private async Task PickOutputDirectory()
	{

		var folder = await _diskDialogService.OpenFolderAsync(
			"Select Output Directory",
			_userPaths.Output);

		if (folder is null || !folder.Exists)
			return;

		_settings.SetOutputFolder(folder);
	}

	[ReactiveCommand]
	private async Task ShowAboutDialogAsync() =>
		await ShowAboutDialog.Handle(AboutDialog);
}
