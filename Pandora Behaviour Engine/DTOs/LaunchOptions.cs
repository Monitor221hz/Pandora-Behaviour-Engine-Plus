using System.IO;

namespace Pandora.DTOs;

public sealed record LaunchOptions(
    DirectoryInfo? OutputDirectory,
    DirectoryInfo? SkyrimGameDirectory,
    bool AutoRun,
    bool AutoClose,
    bool UseSkyrimDebug64
);
