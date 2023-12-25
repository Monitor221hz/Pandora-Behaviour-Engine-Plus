using System;
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

namespace Pandora.Patch.Patchers.Skyrim;

public class SkyrimPatcher : IPatcher
{
	private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


	private List<IModInfo> activeMods { get; set; } = new List<IModInfo>();


    public void SetTarget(List<IModInfo> mods) => activeMods = mods;






    private IAssembler assembler { get; set; } = new NemesisAssembler();


    public string GetPostRunMessages()
    {
        StringBuilder logBuilder = new StringBuilder("Resources loaded successfully.\r\n");

		for (int i = 0; i < activeMods.Count; i++)
        {
			IModInfo mod = activeMods[i];
            string modLine = $"Mod {i + 1} : {mod.Name} - v.{mod.Version}";
			logBuilder.AppendLine(modLine);
            logger.Info(modLine);
        }
        return logBuilder.ToString();
    }

    public void Run()
    {
        //assembler.ApplyPatches();
    }
    public async Task RunAsync()
    {
        await assembler.ApplyPatchesAsync();
    }

    public async Task UpdateAsync()
    {
#if DEBUG
		Parallel.ForEach(activeMods, mod => { assembler.AssemblePatch(mod); });
#else
        try
        {
			Parallel.ForEach(activeMods, mod => { assembler.AssemblePatch(mod); });
		}
        catch (Exception ex)
        {
            logger.Fatal($"Skyrim Patcher > Active Mods > Update > FAILED");
            logger.Error($"Skyrim Patcher > Active Mods > Update > {ex.Message} > EXCEPTION");
			//foreach(var exception in ex.InnerExceptions)
			//{
			//    logger.Error($"Skyrim Patcher > Active Mods > Update > {exception.Message} > EXCEPTION");
			//}
		}
#endif
		//await assembler.LoadResourcesAsync();
       
		//List<Task> assembleTasks = new List<Task>();
		//foreach (var mod in activeMods)
		//{
		//	assembleTasks.Add(Task.Run(() => { assembler.AssemblePatch(mod); }));
		//}
		//await Task.WhenAll(assembleTasks);
	}

    public async Task WriteAsync()
    {

    }

    public void Update()
    {

    }

	public async Task PreloadAsync()
	{
		await assembler.LoadResourcesAsync();
	}
}
