// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.IO;
using Pandora.Models;

namespace Pandora.Utils;

public static class PandoraPaths
{
	private const string ACTIVE_MODS_FILENAME = "ActiveMods.json";
	private const string PREVIOUS_OUTPUT_FILENAME = "PreviousOutput.txt";
	private const string PANDORA_ENGINE_FOLDERNAME = "Pandora_Engine";
	public static DirectoryInfo OutputPath =>
		LaunchOptions.Current?.OutputDirectory
		?? BehaviourEngine.SkyrimGameDirectory
		?? BehaviourEngine.CurrentDirectory;

	public static DirectoryInfo PandoraEngine => _pandoraEngine.Value;
	private static readonly Lazy<DirectoryInfo> _pandoraEngine = new(() =>
	{
		var dir = new DirectoryInfo(Path.Combine(OutputPath.FullName, PANDORA_ENGINE_FOLDERNAME));
		dir.Create();
		return dir;
	});

	public static FileInfo ActiveModsFile =>
		new(Path.Join(PandoraEngine.FullName, ACTIVE_MODS_FILENAME));
	public static FileInfo PreviousOutputFile =>
		new(Path.Join(PandoraEngine.FullName, PREVIOUS_OUTPUT_FILENAME));
}
