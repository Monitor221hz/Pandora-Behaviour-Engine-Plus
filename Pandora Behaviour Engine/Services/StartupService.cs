using Pandora.API.ModManager;
using Pandora.Logging;
using Pandora.Models;
using Pandora.Models.Patch.Plugins;
using Pandora.Utils;

namespace Pandora.Services;

public class StartupService
{
	public record StartupInfo(bool IsCustomSet, bool AutoRun, bool AutoClose, string Message);

	public static StartupInfo Handle(LaunchOptions? options)
	{
		var outputDir = options?.OutputDirectory;
		var manager = ProcessUtils.GetModManagerSource();

		bool isCustom = outputDir is not null || manager == ModManager.ModOrganizer;

		string message = string.Empty;
		if (!isCustom)
		{
			message = manager switch
			{
				ModManager.Vortex => "Output folder not set via -o. In the Pandora tool settings, add the parameter -o to the Command Line field.",
				_ => "Output folder is not set. Use the -o argument to define it, or default location will be used."
			};
		}

		return new StartupInfo(isCustom, options?.AutoRun ?? false, options?.AutoClose ?? false, message);
	}

	public static void LogPlugins()
	{
		if (PluginManager.EngineConfigurations.Count > 0)
		{
			EngineLoggerAdapter.AppendLine("Plugins loaded.");
		}
	}
}
