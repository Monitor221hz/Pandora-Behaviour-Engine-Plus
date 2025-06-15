using System.CommandLine;
using System.CommandLine.Parsing;
using System.IO;

namespace Pandora.Utils;

public class LaunchOptions
{
	public DirectoryInfo? OutputDirectory { get; private set; }
	public DirectoryInfo? SkyrimGameDirectory { get; private set; }
	public bool AutoRun { get; private set; }
	public bool AutoClose { get; private set; }
	public bool UseSkyrimDebug64 { get; private set; }

	public static LaunchOptions Parse(string[] args)
	{
		var options = new LaunchOptions();

		var outputOption = new Option<DirectoryInfo?>(
			aliases: ["--output", "-o"],
			description: "Output directory");

		var tesvOption = new Option<DirectoryInfo?>(
			"--tesv",
			description: "Skyrim directory (TESV), should point to Skyrim root folder");

		var autoRunOption = new Option<bool>(
			"--auto_run",
			description: "Automatically run after start");

		var autoCloseOption = new Option<bool>(
			"--auto_close",
			description: "Close app after execution");

		var skyrimDebug64Option = new Option<bool>(
			"--skyrim_debug64",
			description: "Close app after execution");

		var rootCommand = new RootCommand
		{
			outputOption,
			tesvOption,
			autoRunOption,
			autoCloseOption,
			skyrimDebug64Option
		};

		rootCommand.SetHandler((DirectoryInfo? output, DirectoryInfo? tesv, bool autorun, bool autoclose, bool useskyrimdebug64) =>
		{
			options.OutputDirectory = output;
			options.AutoRun = autorun;
			options.AutoClose = autoclose;
			options.SkyrimGameDirectory = tesv;
			options.UseSkyrimDebug64 = useskyrimdebug64;
		}, outputOption, tesvOption, autoRunOption, autoCloseOption, skyrimDebug64Option);

		rootCommand.Invoke(args);

		return options;
	}
}
