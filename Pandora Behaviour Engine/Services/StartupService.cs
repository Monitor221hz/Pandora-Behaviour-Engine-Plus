using Pandora.API.ModManager;
using Pandora.Logging;
using Pandora.Models;
using Pandora.Models.Patch.Plugins;
using Pandora.Utils;
using System.IO;

namespace Pandora.Services;

public class StartupService
{
	public record StartupInfo(DirectoryInfo OutputDir, bool IsCustomSet, bool AutoRun, bool AutoClose, string Message);

	public static StartupInfo Handle(LaunchOptions? options)
	{
		var outputDir = BehaviourEngine.SkyrimGameDirectory ?? BehaviourEngine.CurrentDirectory;
		var isCustom = false;
		string message = string.Empty;

		ModManager manager = ProcessUtils.GetModManagerSource();

		if (options?.OutputDirectory != null)
		{
			outputDir = options.OutputDirectory;
			isCustom = true;
		}
		else if (manager == ModManager.ModOrganizer)
		{
			isCustom = true;
		}
		else
		{
			message = manager switch
			{
				ModManager.Vortex => "Output folder not set via -o. In the Pandora tool settings, add the parameter -o to the Command Line field.",
				_ => "Output folder is not set. Use the -o argument to define it, or default location will be used."
			};
		}

		return new StartupInfo(outputDir, isCustom, options?.AutoRun ?? false, options?.AutoClose ?? false, message);
	}

	public static void LogPlugins()
	{
		if (PluginManager.EngineConfigurations.Count > 0)
		{
			EngineLoggerAdapter.AppendLine("Plugins loaded.");
		}
	}
}
