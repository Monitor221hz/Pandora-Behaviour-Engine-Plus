// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Text;
using HKX2E;
using Pandora.API.Patch;
using Pandora.API.Patch.Skyrim64;
using Pandora.Core.Paths.Abstractions;
using Pandora.Skyrim.Patch.IO;
using static Pandora.API.Patch.IPatcher;

namespace Pandora.Skyrim;

public sealed record ProjectPackResult(
	string ProjectName,
	int PackFilesExported,
	int PackFilesSerialized,
	string Failures
);

public class PandoraTemplatePacker : IPatcher
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

	private List<IModInfo> _activeMods = [];

	private readonly IProjectManager _projectManager;
	private readonly DebugPackFileExporter _debugPackFileExporter;
	private readonly IEnginePathsFacade _paths;

	private readonly List<ProjectPackResult> _projectResults = [];

	public PatcherFlags Flags { get; private set; } = PatcherFlags.None;

	public PandoraTemplatePacker(
		IEnginePathsFacade paths,
		IProjectManager projectManager,
		DebugPackFileExporter debugPackFileExporter
	)
	{
		_paths = paths;
		_projectManager = projectManager;
		_debugPackFileExporter = debugPackFileExporter;
	}

	public string GetFailureMessages()
	{
		StringBuilder logBuilder = new("TEMPLATE PACK FAILED \r\n\r\n");

		if (Flags.HasFlag(PatcherFlags.LaunchFailed))
		{
			logBuilder.AppendLine(
				"The engine encountered one or more errors while packing templates."
			);
		}

		bool anyFailures = false;
		foreach (var result in _projectResults)
		{
			if (!string.IsNullOrEmpty(result.Failures))
			{
				anyFailures = true;
				logBuilder.Append(result.Failures);
			}
		}

		if (!anyFailures)
		{
			logBuilder.Append(
				"If the cause is unknown: submit a report to the author of the engine and attach Engine.log"
			);
		}

		return logBuilder.ToString();
	}

	public string GetPostRunMessages()
	{
		StringBuilder logBuilder = new("\r\n");

		if (_projectResults.Count == 0)
		{
			logBuilder.AppendLine("No tracked projects found to pack.");
			return logBuilder.ToString();
		}

		int totalExported = 0;
		int totalSerialized = 0;
		int totalFailures = 0;

		foreach (var result in _projectResults)
		{
			logBuilder.AppendLine(
				$"Project \"{result.ProjectName}\" - {result.PackFilesExported} PackFile(s) exported."
			);
			totalExported += result.PackFilesExported;
			totalSerialized += result.PackFilesSerialized;
			if (!string.IsNullOrEmpty(result.Failures))
			{
				totalFailures += result.Failures.Count(c => c == '\n') + 1;
			}
		}

		logBuilder.AppendLine();
		logBuilder.AppendLine($"Total: {totalExported} PackFile(s) exported.");
		if (totalSerialized > 0)
		{
			logBuilder.AppendLine(
				$"Total: {totalSerialized} PackFile(s) re-serialized from XML meta inputs."
			);
		}
		if (totalFailures > 0)
		{
			logBuilder.AppendLine($"{totalFailures} PackFile(s) failed to pack.");
		}

		if (_activeMods.Count > 0)
		{
			logBuilder.AppendLine();
			foreach (var mod in _activeMods)
			{
				string modLine = $"Pandora Mod {mod.Priority} : {mod.Name} - v.{mod.Version}";
				logBuilder.AppendLine(modLine);
				Logger.Info(modLine);
			}
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
		try
		{
			foreach (var project in _projectManager.GetAllProjects())
			{
				_projectResults.Add(await Task.Run(() => PackProject(project)));
			}
		}
		catch (Exception ex)
		{
			Flags |= PatcherFlags.LaunchFailed;
			Logger.Fatal($"Pandora Template Packer > Run > FAILED > {ex}");
			return false;
		}

		return true;
	}

	public void SetTarget(List<IModInfo> mods) => _activeMods = mods;

	public Task<bool> UpdateAsync()
	{
		Logger.Info($"Pandora Template Packer {GetVersionString()}");
		return Task.FromResult(true);
	}

	private ProjectPackResult PackProject(IProject project)
	{
		StringBuilder failures = new();
		int exported = 0;
		int serialized = 0;

		foreach (var packFile in project.GetAllPackFiles())
		{
			try
			{
				_debugPackFileExporter.Export(packFile);
				exported++;

				var inputHandleXml = new FileInfo(
					Path.Join(
						packFile.InputHandle.DirectoryName,
						Path.GetFileNameWithoutExtension(packFile.InputHandle.Name),
						".xml"
					)
				);
				if (inputHandleXml.Exists)
				{
					var metaSerializer = new MetaPackFileSerializer();
					using (var stream = packFile.InputHandle.Create())
					{
						var bw = new BinaryWriterEx(stream);
						metaSerializer.Serialize(packFile.Container, bw, HKXHeader.SkyrimSE());
					}
					serialized++;
				}
			}
			catch (Exception ex)
			{
				string line =
					$"Template Packer > Project \"{project.Identifier}\" > PackFile \"{packFile.Name}\" > FAILED > {ex}";
				failures.AppendLine(line);
				Logger.Warn(line);
			}
		}

		return new ProjectPackResult(project.Identifier, exported, serialized, failures.ToString());
	}
}
