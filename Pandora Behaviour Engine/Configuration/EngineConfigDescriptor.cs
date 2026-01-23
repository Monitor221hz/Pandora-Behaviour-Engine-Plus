using Pandora.API.Patch.Config;

namespace Pandora.Configuration;

public record EngineConfigDescriptor(
    IEngineConfigurationFactory Factory,
    string Name,
    string MenuPath
);
