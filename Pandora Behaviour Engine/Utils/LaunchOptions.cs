using Pandora.Logging;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;

namespace Pandora.Utils;

public class LaunchOptions
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	public DirectoryInfo? OutputDirectory { get; private set; }
	public DirectoryInfo? SkyrimGameDirectory { get; private set; }
	public bool AutoRun { get; private set; }
	public bool AutoClose { get; private set; }
	public bool UseSkyrimDebug64 { get; private set; }

	public static LaunchOptions? Current { get; set; }

	public static LaunchOptions Parse(string[]? args, bool caseInsensitive = false)
	{
		args ??= [];
		var options = new LaunchOptions();

		Option<DirectoryInfo?> outputOption = new(name: "--output", aliases: ["--output", "-o"])
		{
			Description = "Output directory."
		};
		Option<DirectoryInfo?> tesvOption = new(name: "--tesv", aliases: "--tesv")
		{
			Description = "Skyrim directory (TESV), should point to Skyrim root folder."
		};
		Option<bool> autoRunOption = new(name: "--auto_run", aliases: ["--auto_run", "-ar"])
		{
			Description = "Automatically run after start."
		};
		Option<bool> autoCloseOption = new(name: "--auto_close", aliases: ["--auto_close", "-ac"])
		{
			Description = "Close app after execution."
		};
		Option<bool> skyrimDebug64Option = new(name: "--skyrim_debug64", aliases: "--skyrim_debug64")
		{
			Description = "Use skyrim debug 64-bit mode."
		};
		RootCommand rootCommand = new("Pandora command line arguments.");

		rootCommand.Options.Add(outputOption);
		rootCommand.Options.Add(tesvOption);
		rootCommand.Options.Add(autoRunOption);
		rootCommand.Options.Add(autoCloseOption);
		rootCommand.Options.Add(skyrimDebug64Option);

		rootCommand.SetAction(parseResult =>
		{
			options.OutputDirectory = parseResult.GetValue(outputOption);
			options.SkyrimGameDirectory = parseResult.GetValue(tesvOption);
			options.AutoRun = parseResult.GetValue(autoRunOption);
			options.AutoClose = parseResult.GetValue(autoCloseOption);
			options.UseSkyrimDebug64 = parseResult.GetValue(skyrimDebug64Option);
		});

		string[] normalizedArgs = args;
		if (caseInsensitive)
		{
			normalizedArgs = PreprocessArgumentsForCaseInsensitivity(args, rootCommand);
		}

		var config = new CommandLineConfiguration(rootCommand);
		var parseResult = config.Parse(normalizedArgs);

		if (parseResult.Errors.Any())
		{
			var unmatchedTokens = parseResult.UnmatchedTokens;
			if (unmatchedTokens.Any())
			{
				string unrecognizedArgs = string.Join(", ", unmatchedTokens.Select(token => $"'{token}'"));
				string errorArgsMsg = $"Unrecognized command-line argument(s) provided: {unrecognizedArgs}.";
				string infoArgsMsg = "Please check the spelling and prefixes ('-' for short aliases, '--' for full names).";
				logger.Error(errorArgsMsg);
				logger.Info(infoArgsMsg);
				EngineLoggerAdapter.AppendLine($"ERROR: {errorArgsMsg}");
				EngineLoggerAdapter.AppendLine(infoArgsMsg);
			}

			var otherErrors = parseResult.Errors
				.Where(e => !e.Message.StartsWith("Unrecognized command or argument"))
				.ToList();

			if (otherErrors.Any())
			{
				logger.Error("Additional command-line parsing errors found:");
				foreach (var error in otherErrors)
				{
					logger.Error($"- {error.Message}");
				}
				EngineLoggerAdapter.AppendLine("ERROR: Command line arguments could not be processed. See Engine.log for details.");
			}
		}
		else
		{
			parseResult.Invoke();
		}

		Current = options;
		return options;
	}

	/// <summary>
	/// Normalizes command-line arguments to ensure case-insensitive parsing of option aliases.
	/// This method processes input arguments to match option aliases regardless of case and handles
	/// attached values (e.g., --option:value, --option=value, or --optionvalue). It also removes
	/// quotes from quoted values if present. This is a workaround for the lack of native case-insensitive
	/// parsing in System.CommandLine, pending a proposed solution in
	/// <see href="https://github.com/dotnet/command-line-api/pull/2284">PR #2284</see>.
	/// </summary>
	/// <param name="args">The array of command-line arguments to normalize.</param>
	/// <param name="rootCommand">The command containing the defined options and their aliases.</param>
	/// <returns>An array of normalized arguments, with option aliases standardized and values separated.</returns>
	private static string[] PreprocessArgumentsForCaseInsensitivity(string[] args, RootCommand rootCommand)
	{
		var canonicalAliases = rootCommand.Options
			.SelectMany(option => option.Aliases)
			.OrderByDescending(alias => alias.Length)
			.ToList();

		var processedArgs = new List<string>(args.Length);

		foreach (var token in args)
		{

			if (!token.StartsWith('-'))
			{
				processedArgs.Add(token);
				continue;
			}

			var matchedCanonicalAlias = canonicalAliases.FirstOrDefault(
				alias => token.StartsWith(alias, StringComparison.OrdinalIgnoreCase));

			if (matchedCanonicalAlias != null)
			{
				var valuePart = token[matchedCanonicalAlias.Length..];

				if (matchedCanonicalAlias.StartsWith("--") && valuePart.Length > 0 && !valuePart.StartsWith(':') && !valuePart.StartsWith('='))
				{
					processedArgs.Add($"{matchedCanonicalAlias}:{valuePart}");
				}
				else
				{
					processedArgs.Add(matchedCanonicalAlias + valuePart);
				}
			}
			else
			{
				processedArgs.Add(token);
			}
		}
		return [.. processedArgs];
	}
}
