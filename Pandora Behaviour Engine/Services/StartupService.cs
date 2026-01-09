// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.ModManager;
using Pandora.Models.Patch.Plugins;
using Pandora.Utils;

namespace Pandora.Services;

public class StartupService
{

	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	public record StartupInfo(
		bool IsCustomSet,
		bool AutoRun,
		bool AutoClose,
		string Message,
		bool UseSkyrimDebug64
	);

	public static StartupInfo Handle(LaunchOptions? options)
	{
		bool useSkyrimDebug64 = options.UseSkyrimDebug64;
		var outputDir = options?.OutputDirectory;
		var modManager = ProcessUtils.Source;

		bool isCustom = outputDir is not null || modManager == ModManager.ModOrganizer;

		string message = string.Empty;
		if (!isCustom)
		{
			message = modManager switch
			{
				ModManager.Vortex =>
					"Output folder not set via -o. In the Pandora tool settings, add the parameter -o to the Command Line field.",
				_ =>
					"Output folder is not set. Use the -o argument to define it, or default location will be used.",
			};
		}

		return new StartupInfo(
			isCustom,
			options?.AutoRun ?? false,
			options?.AutoClose ?? false,
			message,
			useSkyrimDebug64
		);
	}

	public static void LogPlugins()
	{
		if (PluginManager.EngineConfigurations.Count > 0)
		{
			logger.UiInfo("Plugins loaded.");
		}
	}
}
