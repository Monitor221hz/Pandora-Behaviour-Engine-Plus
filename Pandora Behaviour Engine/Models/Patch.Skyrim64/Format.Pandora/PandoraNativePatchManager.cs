using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Skyrim64;
using Pandora.Models.Patch.Engine.Plugins;
using Pandora.Models.Patch.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Pandora.Models.Patch.Skyrim64.Format.Pandora;

public class PandoraNativePatchManager
{
	private static readonly IPluginLoader pluginLoader = new PluginLoader();
	private readonly List<ISkyrim64Patch> patches = [];
	IEnumerable<IGrouping<RuntimeMode, ISkyrim64Patch>>? patchesByRuntime;

	public void QueuePatches()
	{
		patchesByRuntime = patches.GroupBy(p => p.Mode);
	}
	private IEnumerable<ISkyrim64Patch> Collect(RuntimeMode mode, RunOrder order)
	{
		var grouping = patchesByRuntime!.FirstOrDefault(g => g.Key == mode);
		return grouping == null ? new List<ISkyrim64Patch>() : grouping.Where(p => p.Order == order);
	}
	public void ApplyPatches(IProjectManager manager, RuntimeMode mode, RunOrder order)
	{
		var targets = Collect(mode, order);
		if (targets.Count() == 0) { return; }
		switch (mode)
		{
			case RuntimeMode.Serial:
				foreach (var patch in targets) { patch.Run(manager); }
				break;
			case RuntimeMode.Parallel:
				Parallel.ForEach(targets, t => t.Run(manager));
				break;
		}
	}
	private void RegisterPatches(Assembly assembly)
	{
		foreach (Type type in assembly.GetTypes())
		{
			if (typeof(ISkyrim64Patch).IsAssignableFrom(type) && type.GetConstructor(Type.EmptyTypes) != null)
			{
				if (Activator.CreateInstance(type) is ISkyrim64Patch result)
				{
					patches.Add(result);
				}
			}
		}
	}
	public bool LoadAssembly(DirectoryInfo directoryInfo)
	{
		try
		{
			if (!pluginLoader.TryLoadPlugin(directoryInfo, out var assembly)) { return false; }
			RegisterPatches(assembly);
			return true;
		}
		catch (Exception e)
		{
			return false;
		}
	}
}