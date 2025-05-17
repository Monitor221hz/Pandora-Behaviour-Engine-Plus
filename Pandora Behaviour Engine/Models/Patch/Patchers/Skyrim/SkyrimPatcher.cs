using Pandora.API.Patch;
using Pandora.API.Patch.IOManagers;
using Pandora.Core;
using Pandora.Core.IOManagers;
using Pandora.Core.Patchers;
using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.IOManagers;
using Pandora.Patch.IOManagers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.AnimData;
using Pandora.Patch.Patchers.Skyrim.AnimSetData;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using Pandora.Patch.Patchers.Skyrim.Nemesis;
using Pandora.Patch.Patchers.Skyrim.Pandora;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XmlCake.Linq;
using static Pandora.API.Patch.IPatcher;

namespace Pandora.Patch.Patchers.Skyrim;

public class SkyrimPatcher : IPatcher
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private List<IModInfo> activeMods { get; set; } = [];

	public void SetTarget(List<IModInfo> mods) => activeMods = mods;
	private IMetaDataExporter<PackFile> exporter = new PackFileExporter();

	private NemesisAssembler nemesisAssembler { get; set; }
	private PandoraAssembler pandoraAssembler { get; set; }

	public PatcherFlags Flags { get; private set; } = PatcherFlags.None;

	public Version GetVersion() => Assembly.GetEntryAssembly().GetName().Version;
    public string GetVersionString() => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+')[0] ?? "Unknown";

	public SkyrimPatcher(IMetaDataExporter<PackFile> manager)
	{
		exporter = manager;
		nemesisAssembler = new NemesisAssembler(manager);
		pandoraAssembler = new PandoraAssembler(manager, nemesisAssembler);
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

		nemesisAssembler.GetPostMessages(logBuilder);


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
		return await nemesisAssembler.ApplyPatchesAsync();
	}

	public async Task<bool> UpdateAsync()
	{

		logger.Info($"Skyrim Patcher {GetVersionString()}");

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
		if (!string.Equals(directoryInfo.FullName, BehaviourEngine.AssemblyDirectory.FullName, StringComparison.OrdinalIgnoreCase))
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

	public string GetPostUpdateMessages()
	{
		return string.Empty;
	}
}
