using Pandora.Logging.Extensions;
using Pandora.Models.Engine;
using Pandora.Mods.Services;
using Pandora.Services.Interfaces;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Splat;
using System;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Pandora.ViewModels;

public partial class LaunchElementViewModel : ViewModelBase, IActivatableViewModel
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	public IEngineSharedState State { get; }

	private readonly IModService _modService;
	private readonly IBehaviourEngine _engine;

	private readonly bool _autoClose = false;
	private readonly bool _autoRun = false;

	public ViewModelActivator Activator { get; } = new();

	public LaunchElementViewModel(
		IEngineSharedState state,
		IModService modService,
		IBehaviourEngine engine)
	{
		State = state;
		_engine = engine;
		_modService = modService;

		this.WhenActivated(disposables =>
		{
			LaunchEngineCommand
				.ThrownExceptions.Subscribe(ex => this.Log().Error(ex))
				.DisposeWith(disposables);

			if (_autoRun)
				LaunchEngineCommand.Execute().Subscribe().DisposeWith(disposables);
		});

	}

	[ReactiveCommand]
	private async Task LaunchEngine()
	{
		var activeMods = _modService.GetActiveMods();

		var result = await Task.Run(() => _engine.RunAsync(activeMods));

		logger.UiInfo(result.Message);
		logger.UiInfo($"Launch finished in {result.Duration.TotalSeconds:F2} seconds.");

		await _modService.SaveSettingsAsync();
	}
}
