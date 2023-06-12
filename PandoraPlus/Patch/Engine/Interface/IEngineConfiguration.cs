using Pandora.Patch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace PandoraPlus.Patch
{
    public interface IEngineConfiguration
    {
        string Name { get; }
        string Version { get; }
        string Description { get; }
        IPackFilePatcher Patcher { get; }
    }
}
