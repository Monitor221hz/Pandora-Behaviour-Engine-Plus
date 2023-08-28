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
using XmlCake.Linq;

namespace Pandora.Patch.Patchers.Skyrim.Nemesis;

public class NemesisPatcher : IPatcher
{
    private List<IModInfo> activeMods { get; set; } = new List<IModInfo>();


    public void SetTarget(List<IModInfo> mods) => activeMods = mods;






    private IAssembler assembler { get; set; } = new NemesisAssembler();

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
            assembleTasks.Add(Task.Run(() => { assembler.AssemblePatch(mod.Folder); })); 
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