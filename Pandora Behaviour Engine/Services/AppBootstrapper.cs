using Pandora.DTOs;
using Pandora.Logging;
using Pandora.Logging.Extensions;
using Pandora.Logging.Services;
using Pandora.Models.Engine;
using Pandora.Models.Patch.Plugins;
using Pandora.Mods.Services;
using Pandora.Paths.Services;
using Pandora.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Pandora.Services;

public class AppBootstrapper : IAppBootstrapper
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private readonly IAppExceptionHandler _appExceptionHandler;
	private readonly IEngineConfigurationService _configService;
	private readonly ILoggingConfigurationService _logService;
	private readonly IGamePathService _gamePathService;
	private readonly IOutputPathService _outputPathService;
	private readonly IModService _modService;
	private readonly IBehaviourEngine _engine;
	private readonly EngineOrchestrator _orchestrator;
	private readonly LaunchOptions _launchOptions;

	public AppBootstrapper(
		IAppExceptionHandler appExceptionHandler,
		IEngineConfigurationService configService,
		ILoggingConfigurationService logService,
		IGamePathService gamePathService,
		IOutputPathService outputPathService,
		IModService modService,
		IBehaviourEngine engine,
		EngineOrchestrator orchestrator,
		LaunchOptions launchOptions)
	{
		_appExceptionHandler = appExceptionHandler;
		_configService = configService;
		_logService = logService;
		_gamePathService = gamePathService;
		_outputPathService = outputPathService;
		_modService = modService;
		_engine = engine;
		_orchestrator = orchestrator;
		_launchOptions = launchOptions;
	}

	public void InitializeSync()
	{
		_appExceptionHandler.Initialize();

		_gamePathService.Initialize();
		_outputPathService.Initialize();

		_orchestrator.Initialize();

		_logService.Initialize();

		_configService.Initialize(_launchOptions.UseSkyrimDebug64);
	}

	public async Task InitializeAsync()
	{
		try
		{
			if (PluginManager.EngineConfigurationPlugins.Count > 0)
			{
				logger.UiInfo("Plugins loaded.");
			}

			var loadModsTask = _modService.RefreshModsAsync();
			var initEngineTask = _engine.InitializeAsync();

			await Task.WhenAll(loadModsTask, initEngineTask);
		}
		catch (Exception ex)
		{
			logger.Fatal(ex, "Startup failed");
		}
	}
}