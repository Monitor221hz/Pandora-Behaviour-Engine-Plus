// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.CommandLine;
using System.IO;
using System.Linq;

namespace Pandora.CLI;

public sealed class CommandLineParser
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

	private readonly RootCommand _rootCommand;
	private readonly Option<DirectoryInfo?> _outputOption;
	private readonly Option<DirectoryInfo?> _tesvOption;
	private readonly Option<bool> _autoRunOption;
	private readonly Option<bool> _autoCloseOption;
	private readonly Option<bool> _skyrimDebug64Option;

	public CommandLineParser()
	{
		_outputOption = new("--output", ["-o"])
		{
			Description = "Output directory"
		};

		_tesvOption = new("--tesv", ["-tesv"])
		{
			Description = "Skyrim root directory"
		};

		_autoRunOption = new("--auto_run", ["-ar"]);
		_autoCloseOption = new("--auto_close", ["-ac"]);
		_skyrimDebug64Option = new("--skyrim_debug64");

		_rootCommand = [];
		_rootCommand.Options.Add(_outputOption);
		_rootCommand.Options.Add(_tesvOption);
		_rootCommand.Options.Add(_autoRunOption);
		_rootCommand.Options.Add(_autoCloseOption);
		_rootCommand.Options.Add(_skyrimDebug64Option);
	}

	public LaunchOptions Parse(string[] args)
	{
		args ??= [];
		DirectoryInfo? output = null;
		DirectoryInfo? tesv = null;
		bool autoRun = false;
		bool autoClose = false;
		bool debug64 = false;

		_rootCommand.SetAction(result =>
		{
			output = result.GetValue(_outputOption);
			tesv = result.GetValue(_tesvOption);
			autoRun = result.GetValue(_autoRunOption);
			autoClose = result.GetValue(_autoCloseOption);
			debug64 = result.GetValue(_skyrimDebug64Option);
		});

		PreprocessArguments(args, _rootCommand, true);

		var parseResult = _rootCommand.Parse(args);

		if (parseResult.Errors.Any())
		{
			foreach (var error in parseResult.Errors)
			{
				Logger.Error(error.Message);
			}
		}
		else
		{
			parseResult.Invoke();
		}

		return new LaunchOptions(
			output,
			tesv,
			autoRun,
			autoClose,
			debug64
		);
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
