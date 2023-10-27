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

namespace Pandora.Patch.Patchers.Skyrim.Nemesis;

public class NemesisPatcher : IPatcher
{
    private List<IModInfo> activeMods { get; set; } = new List<IModInfo>();


    public void SetTarget(List<IModInfo> mods) => activeMods = mods;






    private IAssembler assembler { get; set; } = new NemesisAssembler();


    public string GetPostRunMessages()
    {
        StringBuilder logBuilder = new StringBuilder("Resources loaded successfully.\r\n");

		for (int i = 0; i < activeMods.Count; i++)
        {
			IModInfo mod = activeMods[i];
			logBuilder.AppendLine($"Mod {i+1} : {mod.Name} - v.{mod.Version}");
        }
        return logBuilder.ToString();
    }

    public void Run()
    {
        assembler.ApplyPatches();
    }

    public async Task UpdateAsync()
    {
        assembler.LoadResources();


        List<Task> assembleTasks = new List<Task>();
        foreach(var mod in activeMods)
        {
            assembleTasks.Add(Task.Run(() => { assembler.AssemblePatch(mod); })); 
        }
        await Task.WhenAll(assembleTasks);
    }

    public async Task WriteAsync()
    {

    }

    public void Update()
    {

    }

}
