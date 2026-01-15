using Pandora.Logging.Extensions;
using Pandora.Paths.Contexts;
using Pandora.Paths.Services;
using Pandora.Platform.CreationEngine;
using Pandora.Services.Interfaces;
using Pandora.Utils;
using Pandora.ViewModels.Configuration;
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

	private readonly IUserPathContext _userPathContext;
	private readonly IGamePathService _gamePathService;
	private readonly IOutputPathService _outputPathService;

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
		IUserPathContext userPathContext,
		IGamePathService gamePathService,
		IOutputPathService outputPathService,
		IEngineSharedState state,
		IEngineConfigurationService engineConfigService,
		IDiskDialogService diskDialogService,
		IGameDescriptor gameDescriptor,
		SettingsViewModel settingsViewModel,
		AboutDialogViewModel AboutDialogViewModel)
	{
		State = state;
		_userPathContext = userPathContext;
		_gamePathService = gamePathService;
		_outputPathService = outputPathService;

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
			_gamePathService
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
			_userPathContext.GameData,
			["*.exe"]);

		if (file is null)
			return;

		var success = _gamePathService.TrySetGameData(file.Directory!);

		if (!success)
		{
			logger.UiWarn("Selected folder is not a valid game directory.");
		}
	}

	[ReactiveCommand]
	private async Task PickOutputDirectory()
	{

		var folder = await _diskDialogService.OpenFolderAsync(
			"Select Output Directory",
			_userPathContext.Output);

		if (folder is null || !folder.Exists)
			return;

		_outputPathService.SetOutputFolder(folder);
	}

	[ReactiveCommand]
	private async Task ShowAboutDialogAsync() =>
		await ShowAboutDialog.Handle(AboutDialog);
}
