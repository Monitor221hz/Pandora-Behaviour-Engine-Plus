using Pandora.Patch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandoraPlus.Patch.Engine.Configs
{
    public class SkyrimConfig: IEngineConfiguration
    {
        public string Name { get; } = "Skyrim SE/AE";

        public string Version { get; } = "1.0.0";

        public string Description { get; } = 
        @"Engine configuration for Skyrim SE/AE behavior files";

        public IPackFilePatcher Patcher { get; } = new SkyrimPatcher();

    }
}
