﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Pandora.Core;
using Pandora.Core.Patchers;
using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.AnimData;
using XmlCake.Linq;
using Pandora.Patch.Patchers.Skyrim.Nemesis;
using System.Diagnostics.Eventing.Reader;
using System.Security.AccessControl;
using Pandora.Patch.Patchers.Skyrim.Pandora;
using Pandora.Core.IOManagers;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using Pandora.Patch.IOManagers;
using Pandora.Patch.IOManagers.Skyrim;

namespace Pandora.Patch.Patchers.Skyrim;
using PatcherFlags = IPatcher.PatcherFlags;
public class SkyrimPatcher : IPatcher
{
	private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private List<IModInfo> activeMods { get; set; } = new List<IModInfo>();

	public void SetTarget(List<IModInfo> mods) => activeMods = mods;
	private IDataExporter<PackFile> exporter = new PackFileExporter();

	private NemesisAssembler nemesisAssembler { get; set; }

	private PandoraFragmentAssembler pandoraAssembler { get; set; }

	public IPatcher.PatcherFlags Flags { get; private set; } = IPatcher.PatcherFlags.None;

	private static readonly Version currentVersion = new Version(2, 4, 0);

	private static readonly string versionLabel = "beta";
	public string GetVersionString() => $"{currentVersion.ToString()}-{versionLabel}";
	public Version GetVersion() => currentVersion;

	public SkyrimPatcher()
	{
		nemesisAssembler = new NemesisAssembler();
		pandoraAssembler = new PandoraFragmentAssembler(nemesisAssembler);
	}
	public SkyrimPatcher(IMetaDataExporter<PackFile> manager)
	{
		exporter = manager;
		nemesisAssembler = new NemesisAssembler(manager);
		pandoraAssembler = new PandoraFragmentAssembler(nemesisAssembler);
	}
	public string GetPostRunMessages()
	{
		StringBuilder logBuilder;
		logBuilder = new StringBuilder("\r\n");

		for (int i = 0; i < activeMods.Count; i++)
		{
			IModInfo mod = activeMods[i];
			string modLine = $"Pandora Mod {i + 1} : {mod.Name} - v.{mod.Version}";
			logBuilder.AppendLine(modLine);
			logger.Info(modLine);
		}

		nemesisAssembler.GetPostMessages(logBuilder);


		return logBuilder.ToString();
	}

	public string GetFailureMessages()
	{
		StringBuilder logBuilder;
		logBuilder = new StringBuilder("CRITICAL FAILURE \r\n\r\n");

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
		return await nemesisAssembler.ApplyPatchesAsync();
	}

	public async Task<bool> UpdateAsync()
	{

		logger.Info($"Skyrim Patcher {GetVersionString()}");

		//Parallel.ForEach(activeMods, mod => { assembler.AssemblePatch(mod); });

		try
		{
			Parallel.ForEach(activeMods, mod =>
			{
				switch (mod.Format)
				{
					case IModInfo.ModFormat.Nemesis:
						nemesisAssembler.AssemblePatch(mod);
						break;
					case IModInfo.ModFormat.Pandora:
						pandoraAssembler.AssemblePatch(mod);
						break;
					default:
						break;
				}
			}
			);
		}
		catch (Exception ex)
		{
			Flags |= PatcherFlags.UpdateFailed;
			logger.Fatal($"Skyrim Patcher > Active Mods > Update > FAILED > {ex.ToString()}");
		}

		//await assembler.LoadResourcesAsync();

		//List<Task> assembleTasks = new List<Task>();
		//foreach (var mod in activeMods)
		//{
		//	assembleTasks.Add(Task.Run(() => { assembler.AssemblePatch(mod); }));
		//}
		//await Task.WhenAll(assembleTasks);

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
		await nemesisAssembler.LoadResourcesAsync();
	}

	public void SetOutputPath(DirectoryInfo directoryInfo)
	{
		exporter.ExportDirectory = directoryInfo;
		if (!String.Equals(directoryInfo.FullName, BehaviourEngine.AssemblyDirectory.FullName, StringComparison.OrdinalIgnoreCase))
		{
			var FNISPlugin = new FileInfo(Path.Combine(BehaviourEngine.AssemblyDirectory.FullName, "FNIS.esp"));
			var outputFNISPlugin = new FileInfo(Path.Combine(directoryInfo.FullName, "FNIS.esp"));

			if (!outputFNISPlugin.Exists)
			{
				FNISPlugin.CopyTo(outputFNISPlugin.FullName);
			}
		}

		nemesisAssembler.SetOutputPath(directoryInfo);
		pandoraAssembler.SetOutputPath(directoryInfo);
	}
}
