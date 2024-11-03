namespace Pandora.API.Patch.Engine.Config;

public interface IEngineConfiguration
{
    string Name { get; }

    string Description { get; }

    public IPatcher Patcher { get; }

}
