using Pandora.DTOs;
using Pandora.Logging.Extensions;
using Pandora.Services.CreationEngine;
using Pandora.Services.Interfaces;
using Pandora.Utils;
using Pandora.ViewModels.Configuration;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Pandora.ViewModels;

public partial class EngineMenuViewModel : ViewModelBase
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private readonly IPathResolver _pathResolver;
	private readonly IEngineConfigurationService _engineConfigService;
	private readonly ILoggingConfigurationService _logConfigService;
	private readonly IDiskDialogService _diskDialogService;
	private readonly IGameDescriptor _gameDescriptor;

	private readonly IEngineSessionState _state;
	public IEngineSessionState State => _state;

	public LaunchOptions LaunchOptions { get; }

	public AboutDiaLogBoxViewModel AboutDialog { get; }
	public SettingsViewModel Settings { get; }

	public Interaction<AboutDiaLogBoxViewModel, Unit> ShowAboutDialog { get; } = new();

	public ObservableCollection<IEngineConfigurationViewModel> EngineConfigurationViewModels { get; } = [];

	public EngineMenuViewModel(
		IEngineSessionState state,
		IEngineConfigurationService engineConfigService,
		ILoggingConfigurationService logConfigService,
		IPathResolver pathResolver,
		IDiskDialogService diskDialogService,
		IGameDescriptor gameDescriptor,
		LaunchOptions launchOptions,
		SettingsViewModel settingsViewModel,
		AboutDiaLogBoxViewModel aboutDiaLogBoxViewModel)
	{
		_state = state;
		_engineConfigService = engineConfigService;
		_logConfigService = logConfigService;
		_pathResolver = pathResolver;
		_diskDialogService = diskDialogService;
		_gameDescriptor = gameDescriptor;

		AboutDialog = aboutDiaLogBoxViewModel;
		Settings = settingsViewModel;
		LaunchOptions = launchOptions;


		EngineConfigurationViewModels = ConfigurationMenuBuilder.BuildTree(
			_engineConfigService.GetAvailableConfigurations(),
			_engineConfigService
		);

		var gameData = _pathResolver.GetGameDataFolder();
		var assemblyDir = _pathResolver.GetAssemblyFolder();

		State.IsOutputFolderCustomSet = !_pathResolver.GetOutputFolder().Equals(_pathResolver.GetGameDataFolder());


		State.OutputFolderUri = _pathResolver.GetOutputFolder().FullName;

		if (!State.IsOutputFolderCustomSet)
		{
			State.OutputDirectoryMessage = $"Custom output dir not set. Files output to: {_pathResolver.GetGameDataFolder().FullName}";
		}

		if (GameDataDirectoryResolver.Resolve(gameData, _gameDescriptor) is null ||
		gameData.FullName.Equals(assemblyDir.FullName, StringComparison.OrdinalIgnoreCase))
		{
			logger.UiWarn("Game Data directory not found automatically.");

			RxApp.MainThreadScheduler.Schedule(async () =>
			{
				await PickGameDirectory();
			});
		}

	}

	[ReactiveCommand]
	private async Task PickGameDirectory()
	{
		var currentGameData = _pathResolver.GetGameDataFolder();

		var file = await _diskDialogService.OpenFileAsync(
			$"Select {_gameDescriptor.Name} Executable",
			currentGameData,
			patterns: ["*.exe"]);

		if (file is null)
			return;

		if (!_pathResolver.TrySetGameDataFolder(file.Directory!))
		{
			logger.UiWarn("Selected folder is not a valid game directory.");
		}
	}

	[ReactiveCommand]
	private async Task PickOutputDirectory()
	{
		var currentOutput = _pathResolver.GetOutputFolder();

		var folder = await _diskDialogService.OpenFolderAsync("Select Output Directory", currentOutput);

		if (folder == null || !folder.Exists)
			return;

		_pathResolver.SetOutputFolder(folder);

		State.OutputFolderUri = folder.FullName;
		State.IsOutputFolderCustomSet = true;
		logger.UiInfo($"Output directory changed: {_pathResolver.GetOutputFolder().FullName}");
		_logConfigService.UpdateLogPath(folder.FullName);
	}

	[ReactiveCommand]
	private async Task ShowAboutDialogAsync() =>
		await ShowAboutDialog.Handle(AboutDialog);
}
