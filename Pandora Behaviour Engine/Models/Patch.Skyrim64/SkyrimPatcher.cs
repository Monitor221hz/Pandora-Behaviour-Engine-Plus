// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Patch;
using Pandora.API.Patch.IOManagers;
using Pandora.Models.Patch.IO.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Format.Nemesis;
using Pandora.Models.Patch.Skyrim64.Format.Pandora;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using Pandora.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Pandora.API.Patch.IPatcher;

namespace Pandora.Models.Patch.Skyrim64;

public class SkyrimPatcher : IPatcher
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private List<IModInfo> activeMods { get; set; } = [];

	public void SetTarget(List<IModInfo> mods) => activeMods = mods;
	private IMetaDataExporter<PackFile> exporter = new PackFileExporter();

	public NemesisAssembler NemesisAssembler { get; set; }
	public PandoraAssembler PandoraAssembler { get; set; }

	public PatcherFlags Flags { get; private set; } = PatcherFlags.None;

	public Version GetVersion() => Assembly.GetEntryAssembly()!.GetName().Version!;
	public string GetVersionString() => AppInfo.Version;

	public SkyrimPatcher(IMetaDataExporter<PackFile> manager)
	{
		exporter = manager;
		NemesisAssembler = new NemesisAssembler(manager);
		PandoraAssembler = new PandoraAssembler(manager, NemesisAssembler);
	}
	public string GetPostRunMessages()
	{
		StringBuilder logBuilder = new("\r\n");

		for (int i = 0; i < activeMods.Count; i++)
		{
			IModInfo mod = activeMods[i];
			string modLine = $"Pandora Mod {i + 1} : {mod.Name} - v.{mod.Version}";
			logBuilder.AppendLine(modLine);
			logger.Info(modLine);
		}

		NemesisAssembler.GetPostMessages(logBuilder);


		return logBuilder.ToString();
	}

	public string GetFailureMessages()
	{
		StringBuilder logBuilder = new("CRITICAL FAILURE \r\n\r\n");

		if (Flags.HasFlag(PatcherFlags.UpdateFailed)) { logBuilder.AppendLine("Engine had one or more errors while updating."); }

		logBuilder.Append("If the cause is unknown: submit a report to the author of the engine and attach Engine.log");

		return logBuilder.ToString();
	}

	public void Run()
	{
		//assembler.ApplyPatches();
	}
	public async Task<bool> RunAsync()
	{
		return await NemesisAssembler.ApplyPatchesAsync();
	}

	public async Task<bool> UpdateAsync()
	{
		logger.Info($"Skyrim Patcher {GetVersionString()}");

		try
		{
			await Parallel.ForEachAsync(activeMods, async (mod, ct) =>
			{
				switch (mod.Format)
				{
					case IModInfo.ModFormat.Nemesis:
						await Task.Run(() => NemesisAssembler.AssemblePatch(mod), ct);
						break;
					case IModInfo.ModFormat.Pandora:
						await Task.Run(() => PandoraAssembler.AssemblePatch(mod), ct);
						break;
					default:
						break;
				}
			});
		}
		catch (Exception ex)
		{
			Flags |= PatcherFlags.UpdateFailed;
			logger.Fatal($"Skyrim Patcher > Active Mods > Update > FAILED > {ex}");
		}

		return !Flags.HasFlag(PatcherFlags.UpdateFailed);
	}

	public async Task WriteAsync()
	{

	}

	public void Update()
	{

	}

	public async Task PreloadAsync()
	{
		await NemesisAssembler.LoadResourcesAsync();
	}

	public void SetOutputPath(DirectoryInfo directoryInfo)
	{
		exporter.ExportDirectory = directoryInfo;
		if (!string.Equals(directoryInfo.FullName, Environment.CurrentDirectory, StringComparison.OrdinalIgnoreCase))
		{
			var FNISPlugin = new FileInfo(Path.Combine(Environment.CurrentDirectory, "FNIS.esp"));
			var outputFNISPlugin = new FileInfo(Path.Combine(directoryInfo.FullName, "FNIS.esp"));

			if (FNISPlugin.Exists && !outputFNISPlugin.Exists)
			{
				outputFNISPlugin.Directory?.Create();
				FNISPlugin.CopyTo(outputFNISPlugin.FullName);
			}
		}

		NemesisAssembler.SetOutputPath(directoryInfo);
		PandoraAssembler.SetOutputPath(directoryInfo);
	}

	public string GetPostUpdateMessages()
	{
		return string.Empty;
	}
}
