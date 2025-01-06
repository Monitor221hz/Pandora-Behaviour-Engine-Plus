using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;
using Pandora.Core;
using Pandora.Core.IOManagers;
using Pandora.Core.Patchers;
using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.IOManagers.Skyrim;
using Pandora.Patch.Patchers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.Nemesis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Core.Engine.Configs
{
    public class SkyrimConfiguration : IEngineConfiguration
    {
        public string Name { get; } = "Skyrim SE/AE";

        public string Description { get; } = 
        @"Engine configuration for Skyrim SE/AE behavior files";

        public IPatcher Patcher { get; } = new SkyrimPatcher(new PackFileExporter());


	}
}
