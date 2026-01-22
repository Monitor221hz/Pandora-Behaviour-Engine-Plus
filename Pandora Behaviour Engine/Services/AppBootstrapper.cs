using Pandora.DTOs;
using Pandora.Logging.Diagnostics;
using Pandora.Logging.Extensions;
using Pandora.Logging.NLogger;
using Pandora.Models.Engine;
using Pandora.Models.Patch.Plugins;
using Pandora.Mods.Services;
using Pandora.Services.Interfaces;
using Pandora.Services.Settings;
using System;
using System.Threading.Tasks;

namespace Pandora.Services;

public sealed class AppBootstrapper(
	AppExceptionHandler appExceptionHandler,
	LoggingBootstrapper nlogger,
	IEngineConfigurationService configService,
	ISettingsService settings,
	IModService modService,
	IBehaviourEngine engine,
	EngineOrchestrator orchestrator,
	LaunchOptions launchOptions)
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	public void InitializeSync()
	{
		appExceptionHandler.Initialize();

		settings.Initialize();

		nlogger.Initialize();

		orchestrator.Initialize();

		configService.Initialize(launchOptions.UseSkyrimDebug64);
	}

	public async Task InitializeAsync()
	{
		try
		{
			if (PluginManager.EngineConfigurationPlugins.Count > 0)
			{
				logger.UiInfo("Plugins loaded.");
			}

			var loadModsTask = modService.RefreshModsAsync();
			var initEngineTask = engine.InitializeAsync();

			await Task.WhenAll(loadModsTask, initEngineTask);
		}
		catch (Exception ex)
		{
			logger.Fatal(ex, "Startup failed");
		}
	}
}