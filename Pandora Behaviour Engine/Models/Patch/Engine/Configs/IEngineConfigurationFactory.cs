using Pandora.Core;
using System.ComponentModel;

namespace Pandora.Core.Engine.Configs;

public interface IEngineConfigurationFactory
{
    public string Name { get; }
    public IEngineConfiguration? Config { get; }
    public bool Selectable => Config != null;
}
