using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.API.Patch.Engine.Config;
public interface IEngineConfigurationFactory
{
    public string Name { get; }
    public IEngineConfiguration? Config { get; }
    public bool Selectable => Config != null;
}
