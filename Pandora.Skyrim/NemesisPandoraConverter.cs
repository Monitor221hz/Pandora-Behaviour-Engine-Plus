// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using Pandora.API.Patch;
using Pandora.API.Patch.Skyrim64;
using Pandora.Core.Patch.Mod;
using Pandora.Core.Paths.Abstractions;
using Pandora.Skyrim.Format.Nemesis;
using Pandora.Skyrim.Hkx.Changes;
using static Pandora.API.Patch.IPatcher;

namespace Pandora.Skyrim;

public sealed record ModConversionResult(string ModName, int Successes, string Failures);

public class NemesisPandoraConverter : IPatcher
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

	private readonly XmlSerializer _serializer = new(typeof(PandoraModInfo));

	private readonly IProjectManager _projectManager;
	private readonly IEnginePathsFacade _paths;

	private readonly List<NemesisModInfo> _nemesisModInfos = [];

	private readonly List<ModConversionResult> _modResults = [];

	public PatcherFlags Flags { get; set; } = PatcherFlags.None;

	public NemesisPandoraConverter(IEnginePathsFacade paths, IProjectManager projectManager)
	{
		_paths = paths;
		_projectManager = projectManager;
	}

	public string GetFailureMessages()
	{
		StringBuilder logBuilder = new();
		bool anyFailures = false;

		foreach (var result in _modResults)
		{
			if (!string.IsNullOrEmpty(result.Failures))
			{
				anyFailures = true;
				logBuilder.Append(result.Failures);
			}
		}

		return anyFailures ? logBuilder.ToString() : string.Empty;
	}

	public string GetPostRunMessages()
	{
		StringBuilder logBuilder = new("\r\n");

		if (_nemesisModInfos.Count == 0)
		{
			logBuilder.AppendLine("No Nemesis patches found in the target mods.");
			return logBuilder.ToString();
		}

		int totalSuccesses = 0;
		int totalFailures = 0;

		foreach (var result in _modResults)
		{
			logBuilder.AppendLine(
				$"Nemesis Mod \"{result.ModName}\" - {result.Successes} patch(es) converted."
			);
			totalSuccesses += result.Successes;
			if (!string.IsNullOrEmpty(result.Failures))
			{
				totalFailures += result.Failures.Count(c => c == '\n') + 1;
			}
		}

		logBuilder.AppendLine();
		logBuilder.AppendLine(
			$"Total: {totalSuccesses} Nemesis patch(es) converted to Pandora patches."
		);
		if (totalFailures > 0)
		{
			logBuilder.AppendLine($"{totalFailures} patch(es) failed to convert.");
		}

		return logBuilder.ToString();
	}

	public Version GetVersion() => new(1, 0, 0);

	public string GetVersionString() => GetVersion().ToString();

	public async Task PreloadAsync()
	{
		await Task.Run(_projectManager.LoadTrackedProjects);
	}

	public async Task<bool> RunAsync()
	{
		var results = new ModConversionResult[_nemesisModInfos.Count];
		await Parallel.ForEachAsync(
			_nemesisModInfos.Select((mod, i) => (mod, i)),
			async (item, cancellationToken) =>
			{
				results[item.i] = await Task.Run(() => AssemblePatch(item.mod), cancellationToken);
			}
		);

		foreach (var result in results)
		{
			_modResults.Add(result);
		}

		return true;
	}

	public void SetTarget(List<IModInfo> mods)
	{
		foreach (var mod in mods)
		{
			if (mod is NemesisModInfo nemesisModInfo)
			{
				_nemesisModInfos.Add(nemesisModInfo);
			}
		}
	}

	public async Task<bool> UpdateAsync()
	{
		return true;
	}

	private int AssemblePackFilePatch(
		DirectoryInfo folder,
		IModInfo modInfo,
		PandoraModInfo newModInfo,
		StringBuilder failures
	)
	{
		IPackFile targetPackFile;
		if (!_projectManager.TryActivatePackFilePriority(folder.Name, out targetPackFile!))
		{
			IProject targetProject;
			if (!_projectManager.TryLookupProjectFolder(folder.Name, out targetProject!))
			{
				return 0;
			}

			int successes = 0;
			DirectoryInfo[] subFolders = folder.GetDirectories();
			foreach (DirectoryInfo subFolder in subFolders)
			{
				successes += AssemblePackFilePatch(
					subFolder,
					targetProject,
					modInfo,
					newModInfo,
					failures
				);
			}
			return successes;
		}
		List<XElement> newElements = new();
		bool success;
		lock (targetPackFile)
		{
			success = NemesisParser
				.ParsePackFileChanges(targetPackFile, modInfo, folder, newElements)
				.SerializePandoraEdits(newModInfo, targetPackFile, newElements);
		}

		if (!success)
		{
			string line =
				$"Nemesis Converter > Mod \"{modInfo.Name}\" > PackFile \"{targetPackFile.Name}\" > Serialize > FAILED > No parent project";
			failures.AppendLine(line);
			Logger.Warn(line);
		}

		return success ? 1 : 0;
	}

	private int AssemblePackFilePatch(
		DirectoryInfo folder,
		IProject project,
		IModInfo modInfo,
		PandoraModInfo newModInfo,
		StringBuilder failures
	)
	{
		IPackFile targetPackFile;
		if (!_projectManager.TryActivatePackFilePriority(folder.Name, project, out targetPackFile!))
		{
			return 0;
		}
		List<XElement> newElements = new();
		bool success;
		lock (targetPackFile)
		{
			success = NemesisParser
				.ParsePackFileChanges(targetPackFile, modInfo, folder, newElements)
				.SerializePandoraEdits(newModInfo, targetPackFile, newElements);
		}

		if (!success)
		{
			string line =
				$"Nemesis Converter > Mod \"{modInfo.Name}\" > PackFile \"{targetPackFile.Name}\" > Serialize > FAILED > No parent project";
			failures.AppendLine(line);
			Logger.Warn(line);
		}

		return success ? 1 : 0;
	}

	private ModConversionResult AssemblePatch(IModInfo modInfo)
	{
		StringBuilder failures = new();

		DirectoryInfo newDirectory = new(
			Path.Combine(_paths.OutputEngineFolder.FullName, "mod", "pandora", modInfo.Code)
		);
		if (newDirectory.Exists)
		{
			newDirectory.Delete(true);
		}
		newDirectory.Create();

		var newModInfo = new PandoraModInfo(
			modInfo.Name,
			modInfo.Author,
			modInfo.URL,
			modInfo.Code,
			modInfo.Version,
			newDirectory
		);

		DirectoryInfo folder = modInfo.Folder;
		DirectoryInfo[] subFolders = folder.GetDirectories();

		int successes = 0;

		foreach (DirectoryInfo subFolder in subFolders)
		{
			int converted = AssemblePackFilePatch(subFolder, modInfo, newModInfo, failures);
			if (converted > 0)
			{
				successes += converted;
				continue;
			}

			if (subFolder.Name.StartsWith("animationsetdata", StringComparison.OrdinalIgnoreCase))
			{
				string destPath = Path.Combine(newModInfo.Folder.FullName, subFolder.Name);
				CopyDirectory(subFolder, destPath);
			}
		}
		var modInfoFile = new FileInfo(Path.Combine(newModInfo.Folder.FullName, "info.xml"));
		using (var stream = modInfoFile.Create())
		{
			_serializer.Serialize(stream, newModInfo);
		}
		return new ModConversionResult(modInfo.Name, successes, failures.ToString());
	}

	private static void CopyDirectory(DirectoryInfo source, string destinationPath)
	{
		Directory.CreateDirectory(destinationPath);
		foreach (FileInfo file in source.GetFiles())
		{
			file.CopyTo(Path.Combine(destinationPath, file.Name), true);
		}
		foreach (DirectoryInfo subDir in source.GetDirectories())
		{
			CopyDirectory(subDir, Path.Combine(destinationPath, subDir.Name));
		}
	}
}
