using Pandora.Core;
using Pandora.Core.IOManagers;
using Pandora.Core.Patchers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Core
{
    public interface IEngineConfiguration
    {
        string Name { get; }

        string Description { get; }

		public IPatcher Patcher { get; }

	}
}
