using Pandora.API.Patch.Config;

namespace Pandora.DTOs;

public record EngineConfigDescriptor(
    IEngineConfigurationFactory Factory,
    string Name,
    string MenuPath
);
