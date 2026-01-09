// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Data;
using Pandora.API.DTOs;
using Pandora.Utils;

namespace Pandora.Services;

public record StartupInfo
{
	public bool AutoRun { get; }
	public bool AutoClose { get; }
	public bool UseSkyrimDebug64 { get; }
	public bool IsOutputCustomSet { get; }
	public string OutputMessage { get; }

	public StartupInfo(LaunchOptions options)
	{
		AutoRun = options.AutoRun;
		AutoClose = options.AutoClose;
		UseSkyrimDebug64 = options.UseSkyrimDebug64;

		var modManager = ProcessUtils.Source;
		var hasOutputArg = options.OutputDirectory != null;

		IsOutputCustomSet = hasOutputArg || modManager == ModManager.ModOrganizer;
		OutputMessage = BuildMessage(IsOutputCustomSet, modManager);
	}

	private static string BuildMessage(bool isCustom, ModManager modManager)
	{
		if (isCustom) return string.Empty;

		return modManager switch
		{
			ModManager.Vortex => "Output folder not set via -o. In Pandora tool settings, add -o to Command Line.",
			_ => "Output folder is not set. Use -o argument or default location will be used.",
		};
	}
}
