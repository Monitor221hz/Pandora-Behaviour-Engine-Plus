// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.API.Patch;
using Pandora.API.Patch.IOManagers;
using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Format.Nemesis;
using Pandora.Models.Patch.Skyrim64.Format.Pandora;
using Pandora.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Pandora.API.Patch.IPatcher;

namespace Pandora.Models.Patch.Skyrim64;

public class NemesisPatcher : IPatcher
{
	public PatcherFlags Flags => throw new NotImplementedException();

	public string GetFailureMessages()
	{
		throw new NotImplementedException();
	}

	public string GetPostRunMessages()
	{
		throw new NotImplementedException();
	}

	public Version GetVersion()
	{
		throw new NotImplementedException();
	}

	public string GetVersionString()
	{
		throw new NotImplementedException();
	}

	public Task PreloadAsync()
	{
		throw new NotImplementedException();
	}

	public Task<bool> RunAsync()
	{
		throw new NotImplementedException();
	}

	public void SetTarget(List<IModInfo> mods)
	{
		throw new NotImplementedException();
	}

	public Task<bool> UpdateAsync()
	{
		throw new NotImplementedException();
	}
}

public class SkyrimPatcher : IPatcher
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private List<IModInfo> activeMods { get; set; } = [];

	private readonly NemesisAssembler _nemesisAssembler;
	private readonly PandoraAssembler _pandoraAssembler;

	public void SetTarget(List<IModInfo> mods) => activeMods = mods;

	private IMetaDataExporter<IPackFile> exporter;

	public PatcherFlags Flags { get; private set; } = PatcherFlags.None;

	public Version GetVersion() => Assembly.GetEntryAssembly()!.GetName().Version!;

	public string GetVersionString() => AppInfo.Version;

	public SkyrimPatcher(
		NemesisAssembler nemesisAssembler,
		PandoraAssembler pandoraAssembler)
	{
		_nemesisAssembler = nemesisAssembler;
		_pandoraAssembler = pandoraAssembler;
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

		_nemesisAssembler.GetPostMessages(logBuilder);

		return logBuilder.ToString();
	}

	public string GetFailureMessages()
	{
		StringBuilder logBuilder = new("CRITICAL FAILURE \r\n\r\n");

		if (Flags.HasFlag(PatcherFlags.UpdateFailed))
		{
			logBuilder.AppendLine("Engine had one or more errors while updating.");
		}

		logBuilder.Append(
			"If the cause is unknown: submit a report to the author of the engine and attach Engine.log"
		);

		return logBuilder.ToString();
	}

	public void Run()
	{
		//assembler.ApplyPatches();
	}

	public async Task<bool> RunAsync()
	{
		return await _nemesisAssembler.ApplyPatchesAsync();
	}

	public async Task<bool> UpdateAsync()
	{
		logger.Info($"Skyrim Patcher {GetVersionString()}");

		try
		{
			Parallel.ForEach(
				activeMods,
				mod =>
				{
					switch (mod.Format)
					{
						case IModInfo.ModFormat.Nemesis:
							_nemesisAssembler.AssemblePatch(mod);
							break;
						case IModInfo.ModFormat.Pandora:
							_pandoraAssembler.AssemblePatch(mod);
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
			logger.Fatal($"Skyrim Patcher > Active Mods > Update > FAILED > {ex}");
		}

		return !Flags.HasFlag(PatcherFlags.UpdateFailed);
	}

	public async Task WriteAsync() { }

	public void Update() { }

	public async Task PreloadAsync()
	{
		await _nemesisAssembler.LoadResourcesAsync();
	}

	public string GetPostUpdateMessages()
	{
		return string.Empty;
	}
}
