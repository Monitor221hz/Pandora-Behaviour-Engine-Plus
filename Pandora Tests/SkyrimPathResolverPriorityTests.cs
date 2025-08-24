using Microsoft.Win32;
using Pandora.Utils;

namespace PandoraTests;

public class SkyrimPathResolverPriorityTests : IDisposable
{
    private readonly ITestOutputHelper _output;

    public readonly DirectoryInfo PrimaryValidSkyrimDir;
    public readonly string PrimaryValidDataPath;

    public readonly DirectoryInfo SecondaryValidSkyrimDir;
    public readonly string SecondaryValidDataPath;

    public readonly DirectoryInfo InvalidDir;
    private readonly string _originalCurrentDirectory;
    public const string TestRegistrySubKey = @"SOFTWARE\PandoraTest\Skyrim Special Edition";

    public SkyrimPathResolverPriorityTests(ITestOutputHelper output)
    {
        _output = output;

        _originalCurrentDirectory = Environment.CurrentDirectory;

        // 1. Primary valid path
        var primaryRoot = Path.Combine(Path.GetTempPath(), $"Skyrim SE");
        PrimaryValidSkyrimDir = new DirectoryInfo(primaryRoot);
        PrimaryValidDataPath = Path.Combine(primaryRoot, "Data");
        Directory.CreateDirectory(PrimaryValidDataPath);
        File.Create(Path.Combine(primaryRoot, "SkyrimSE.exe")).Close();

        // 2. Secondary valid path
        var secondaryRoot = Path.Combine(Path.GetTempPath(), $"Skyrim");
        SecondaryValidSkyrimDir = new DirectoryInfo(secondaryRoot);
        SecondaryValidDataPath = Path.Combine(secondaryRoot, "Data");
        Directory.CreateDirectory(SecondaryValidDataPath);
        File.Create(Path.Combine(secondaryRoot, "SkyrimSELauncher.exe")).Close();

        // 3. Invalid path without .exe
        var invalidRoot = Path.Combine(Path.GetTempPath(), $"Fallout");
        InvalidDir = new DirectoryInfo(Path.Combine(invalidRoot, "Data"));
        Directory.CreateDirectory(InvalidDir.FullName);
    }


    [Fact(DisplayName = "Priority 1: Command line arguments (--tesv)")]
    public void Resolve_Should_PrioritizeCommandLineArgs()
    {
        LaunchOptions.Parse(["--tesv", PrimaryValidSkyrimDir.FullName], caseInsensitive: true);
        Environment.CurrentDirectory = SecondaryValidSkyrimDir.FullName;
        SetRegistryPath(InvalidDir.FullName);

        _output.WriteLine($"Args: {PrimaryValidSkyrimDir.FullName}");
        _output.WriteLine($"Current Dir: {Environment.CurrentDirectory}");
        _output.WriteLine($"Registry: {InvalidDir.FullName}");

        var result = SkyrimPathResolver.Resolve();

        _output.WriteLine($"\nResolved path: {result.FullName}");
        Assert.Equal(PrimaryValidDataPath, result.FullName);
    }

    [Fact(DisplayName = "Priority 2: Current directory (Start In)")]
    public void Resolve_Should_PrioritizeStartIn_When_ArgsAreMissing()
    {
        LaunchOptions.Current = null;
        Environment.CurrentDirectory = PrimaryValidSkyrimDir.FullName;
        SetRegistryPath(SecondaryValidSkyrimDir.FullName);

        _output.WriteLine($"Args: <null>");
        _output.WriteLine($"Current Dir: {Environment.CurrentDirectory}");
        _output.WriteLine($"Registry: {SecondaryValidSkyrimDir.FullName}");

        var result = SkyrimPathResolver.Resolve();

        _output.WriteLine($"\nResolved path: {result.FullName}");
        Assert.Equal(PrimaryValidDataPath, result.FullName);
    }

    [Fact(DisplayName = "Priority 3: Registry")]
    public void Resolve_Should_UseRegistry_When_ArgsAndCurrentDirAreInvalid()
    {
        LaunchOptions.Current = null;
        Environment.CurrentDirectory = InvalidDir.FullName;

        SetRegistryPath(PrimaryValidSkyrimDir.FullName);

        SkyrimPathResolver.PathProvider registryProvider = () => SkyrimPathResolver.TryGetDataPathFromRegistry(
            Registry.CurrentUser,
            TestRegistrySubKey
        );

        SkyrimPathResolver.PathProvider[] providers =
        [
            () => null,
            () => InvalidDir,
            registryProvider 
        ];

        _output.WriteLine($"Args: <null>");
        _output.WriteLine($"Current Dir: {Environment.CurrentDirectory}");
        _output.WriteLine($"Registry: {PrimaryValidSkyrimDir.FullName}");

        var result = providers
            .Select(provider => provider())
            .FirstOrDefault(SkyrimPathResolver.IsValidSkyrimDataDirectory);

        _output.WriteLine($"\nResolved path: {result.FullName}");

        Assert.NotNull(result);
        Assert.Equal(PrimaryValidDataPath, result.FullName);
    }

    [Fact(DisplayName = "Extreme case: Nothing found")]
    public void Resolve_Should_ReturnCurrentDirectory_When_NoValidPathIsFound()
    {
        LaunchOptions.Current = null;

        Environment.CurrentDirectory = InvalidDir.FullName;

        ClearRegistryPath();

        _output.WriteLine($"Args: <null>");
        _output.WriteLine($"Current Dir set to: {Environment.CurrentDirectory}");
        _output.WriteLine($"Registry is empty.");

        var result = SkyrimPathResolver.Resolve();

        _output.WriteLine($"\nResolved path: {result.FullName}");

        Assert.Equal(InvalidDir.FullName, result.FullName);
    }

    public void SetRegistryPath(string path)
    {
        using var key = Registry.CurrentUser.CreateSubKey(TestRegistrySubKey, true);
        key.SetValue("Installed Path", path);
    }

    public void ClearRegistryPath()
    {
        using var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PandoraTest", true);
        key?.DeleteSubKeyTree("Skyrim Special Edition", false);
    }
    public void Dispose()
    {
        Environment.CurrentDirectory = _originalCurrentDirectory;
        LaunchOptions.Current = null;
        ClearRegistryPath();

        if (Directory.Exists(PrimaryValidSkyrimDir.FullName)) Directory.Delete(PrimaryValidSkyrimDir.FullName, true);
        if (Directory.Exists(SecondaryValidSkyrimDir.FullName)) Directory.Delete(SecondaryValidSkyrimDir.FullName, true);
        if (Directory.Exists(InvalidDir.FullName)) Directory.Delete(InvalidDir.FullName, true);
    }
}