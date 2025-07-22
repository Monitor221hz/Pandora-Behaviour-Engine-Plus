using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;
using System.Linq;

namespace Pandora.Utils;

public class LaunchOptions
{
	public DirectoryInfo? OutputDirectory { get; private set; }
	public DirectoryInfo? SkyrimGameDirectory { get; private set; }
	public bool AutoRun { get; private set; }
	public bool AutoClose { get; private set; }
	public bool UseSkyrimDebug64 { get; private set; }

	private static readonly bool CaseInsensitive = true;

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
			aliases: ["--auto_run", "-ar"],
			description: "Automatically run after start");

		var autoCloseOption = new Option<bool>(
			aliases: ["--auto_close", "-ac"],
			description: "Close app after execution");

		var skyrimDebug64Option = new Option<bool>(
			"--skyrim_debug64",
			description: "Use skyrim debug 64-bit mode");

		var rootCommand = new RootCommand
		{
			outputOption,
			tesvOption,
			autoRunOption,
			autoCloseOption,
			skyrimDebug64Option
		};

		rootCommand.SetHandler((output, tesv, autorun, autoclose, useskyrimdebug64) =>
		{
			options.OutputDirectory = output;
			options.SkyrimGameDirectory = tesv;
			options.AutoRun = autorun;
			options.AutoClose = autoclose;
			options.UseSkyrimDebug64 = useskyrimdebug64;
		}, outputOption, tesvOption, autoRunOption, autoCloseOption, skyrimDebug64Option);

		var builder = new CommandLineBuilder(rootCommand)
			.AddMiddleware(CaseInsensitiveMiddleware, MiddlewareOrder.ExceptionHandler)
			.UseDefaults();

		var parser = builder.Build();
		parser.Invoke(args);

		return options;
	}

	private static void CaseInsensitiveMiddleware(InvocationContext context)
	{
		if (!CaseInsensitive) return;

		var commandOptions = context.ParseResult.CommandResult.Command.Options
			.Concat(context.ParseResult.RootCommandResult.Command.Options);

		var allAliases = commandOptions.SelectMany(option => option.Aliases);

		string[] GetCorrectTokenCase(string[] tokens)
		{
			var newTokens = new List<string>(tokens.Length);

			for (int i = 0; i < tokens.Length; i++)
			{
				var token = tokens[i];

				var matchingAlias = allAliases.FirstOrDefault(
					alias => alias.Equals(token, StringComparison.InvariantCultureIgnoreCase));

				if (matchingAlias != null)
				{
					newTokens.Add(matchingAlias);
					continue;
				}

				if (i > 0)
				{
					var previousToken = tokens[i - 1];

					var matchingOption = commandOptions.FirstOrDefault(
						option => option.Aliases.Contains(previousToken, StringComparer.InvariantCultureIgnoreCase));

					if (matchingOption != null)
					{
						var completions = matchingOption.GetCompletions()
							.Select(c => c.InsertText)
							.ToArray();

						if (completions.Length > 0)
						{
							var matchingCompletion = completions.FirstOrDefault(
								completion => token.Equals(completion, StringComparison.InvariantCultureIgnoreCase));
							newTokens.Add(matchingCompletion ?? token);
						}
						else
						{
							newTokens.Add(token);
						}
					}
					else
					{
						newTokens.Add(token);
					}
				}
				else
				{
					newTokens.Add(token);
				}
			}

			return [.. newTokens];
		}

		var tokens = context.ParseResult.Tokens.Select(token => token.Value).ToArray();
		string[] newTokens;

		if (tokens.Length == 0)
		{
			newTokens = [];
		}
		else if (tokens.Length == 1)
		{
			newTokens = [tokens[0].ToLowerInvariant()];
		}
		else
		{
			if (tokens[0].StartsWith('-'))
			{
				newTokens = tokens;
			}
			else
			{
				newTokens = [
						tokens[0].ToLowerInvariant(),
						tokens[1].ToLowerInvariant(),
						.. GetCorrectTokenCase([.. tokens.Skip(2)]),
					];
			}
		}

		context.ParseResult = context.Parser.Parse(newTokens);
	}
}
