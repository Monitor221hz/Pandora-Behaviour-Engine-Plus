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
using XmlCake.Linq;

namespace Pandora.Patch.Patchers.Skyrim.Nemesis;

public class NemesisPatcher : IPatcher
{
    private List<IModInfo> activeMods { get; set; } = new List<IModInfo>();


    public void SetTarget(List<IModInfo> mods) => activeMods = mods;


    private ProjectManager projectManager { get; set; } = new ProjectManager();

    private IDispatcher<XMap, XPathLookup, List<XNode>>  dispatcher { get; set; } = new HkxDispatcher();

    private IAssembler assembler { get; set; } = new NemesisAssembler();
    public void Apply()
    {
        return;
    }

    public void Update()
    {

    }



}
