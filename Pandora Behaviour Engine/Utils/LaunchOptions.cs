// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using Pandora.Logging;

namespace Pandora.Utils;

public class LaunchOptions
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private static readonly RootCommand _rootCommand;
	private static readonly Option<DirectoryInfo?> _outputOption;
	private static readonly Option<DirectoryInfo?> _tesvOption;
	private static readonly Option<bool> _autoRunOption;
	private static readonly Option<bool> _autoCloseOption;
	private static readonly Option<bool> _skyrimDebug64Option;

	public DirectoryInfo? OutputDirectory { get; private set; }
	public DirectoryInfo? SkyrimGameDirectory { get; private set; }
	public bool AutoRun { get; private set; }
	public bool AutoClose { get; private set; }
	public bool UseSkyrimDebug64 { get; private set; }

	public static LaunchOptions? Current { get; set; }

	static LaunchOptions()
	{
		_outputOption = new(name: "--output", aliases: ["-output", "-o"])
		{
			Description = "Output directory.",
		};
		_tesvOption = new(name: "--tesv", aliases: "-tesv")
		{
			Description = "Skyrim directory (TESV), should point to Skyrim root folder.",
		};
		_autoRunOption = new(name: "--auto_run", aliases: ["-auto_run", "-ar"])
		{
			Description = "Automatically run after start.",
		};
		_autoCloseOption = new(name: "--auto_close", aliases: ["-auto_close", "-ac"])
		{
			Description = "Close app after execution.",
		};
		_skyrimDebug64Option = new(name: "--skyrim_debug64", aliases: "-skyrim_debug64")
		{
			Description = "Use skyrim debug 64-bit mode.",
		};

		_rootCommand = new RootCommand("Pandora command line arguments.");
		_rootCommand.Options.Add(_outputOption);
		_rootCommand.Options.Add(_tesvOption);
		_rootCommand.Options.Add(_autoRunOption);
		_rootCommand.Options.Add(_autoCloseOption);
		_rootCommand.Options.Add(_skyrimDebug64Option);
	}

	public static LaunchOptions Parse(string[]? args, bool caseInsensitive = false)
	{
		args ??= [];
		var options = new LaunchOptions();

		_rootCommand.SetAction(parseResult =>
		{
			options.OutputDirectory = parseResult.GetValue(_outputOption);
			options.SkyrimGameDirectory = parseResult.GetValue(_tesvOption);
			options.AutoRun = parseResult.GetValue(_autoRunOption);
			options.AutoClose = parseResult.GetValue(_autoCloseOption);
			options.UseSkyrimDebug64 = parseResult.GetValue(_skyrimDebug64Option);
		});

		PreprocessArguments(args, _rootCommand, caseInsensitive);

		var config = new CommandLineConfiguration(_rootCommand);
		var parseResult = config.Parse(args);

		if (parseResult.Errors.Any())
		{
			var unmatchedTokens = parseResult.UnmatchedTokens;
			if (unmatchedTokens.Any())
			{
				string unrecognizedArgs = string.Join(
					", ",
					unmatchedTokens.Select(token => $"'{token}'")
				);
				string errorArgsMsg =
					$"Unrecognized command-line argument(s) provided: {unrecognizedArgs}.";
				string infoArgsMsg =
					"Please check the spelling and prefixes ('-' for short aliases, '--' for full names).";
				logger.Error(errorArgsMsg);
				logger.Info(infoArgsMsg);
				EngineLoggerAdapter.AppendLine($"ERROR: {errorArgsMsg}");
				EngineLoggerAdapter.AppendLine(infoArgsMsg);
			}

			var otherErrors = parseResult
				.Errors.Where(e => !e.Message.StartsWith("Unrecognized command or argument"))
				.ToList();

			if (otherErrors.Any())
			{
				logger.Error("Additional command-line parsing errors found:");
				foreach (var error in otherErrors)
				{
					logger.Error($"- {error.Message}");
				}
				EngineLoggerAdapter.AppendLine(
					"ERROR: Command line arguments could not be processed. See Engine.log for details."
				);
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
	/// <param name="caseInsensitive">Boolean value to enable case insensitivity.</param>
	private static void PreprocessArguments(
		string[] args,
		RootCommand rootCommand,
		bool caseInsensitive = false
	)
	{
		var comparison = caseInsensitive
			? StringComparison.OrdinalIgnoreCase
			: StringComparison.Ordinal;

		var canonicalAliases = rootCommand
			.Options.SelectMany(option => option.Aliases.Prepend(option.Name))
			.OrderByDescending(alias => alias.Length)
			.ToList();

		for (int i = 0; i < args.Length; i++)
		{
			var token = args[i];
			if (!token.StartsWith('-'))
				continue;

			var matchedCanonicalAlias = canonicalAliases.FirstOrDefault(alias =>
				token.StartsWith(alias, comparison)
			);

			if (matchedCanonicalAlias is null)
				continue;

			var valuePart = token.AsSpan(matchedCanonicalAlias.Length);
			if (valuePart.Length > 0)
			{
				var firstChar = valuePart[0];
				if (firstChar is not ':' and not '=')
				{
					args[i] = $"{matchedCanonicalAlias}:{valuePart}";
					continue;
				}
			}

			args[i] = string.Concat(matchedCanonicalAlias.AsSpan(), valuePart);
		}
	}
}
