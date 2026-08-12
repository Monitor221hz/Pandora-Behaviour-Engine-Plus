namespace Pandora.API.Patch.Config;

public interface IEngineConfigurationPlugin
{
    public enum OptionFlags
    {
        None = 0,
        HidePatches = 1,
    }

    public string DisplayName { get; }
    public string MenuPath { get; }
    public IEngineConfigurationFactory Factory { get; }
}
