using System.IO;

namespace Pandora.CLI;

public sealed record LaunchOptions(
    DirectoryInfo? OutputDirectory,
    DirectoryInfo? SkyrimGameDirectory,
    bool AutoRun,
    bool AutoClose,
    bool UseSkyrimDebug64
);
