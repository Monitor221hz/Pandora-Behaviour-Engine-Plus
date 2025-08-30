// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Microsoft.Win32;
using NSubstitute;
using Pandora.Utils;
using Pandora.Utils.Platform.Windows;
using Pandora.Utils.Skyrim;
using System.Runtime.Versioning;

namespace PandoraTests;

[SupportedOSPlatform("windows")]
public class SkyrimPathResolverPriorityTests : IDisposable
{
    private readonly ITestOutputHelper _output;
    private readonly IRuntimeEnvironment _mockEnvironment;
    private readonly IRegistry _mockRegistry;

    public readonly DirectoryInfo PrimaryValidSkyrimDir;
    public readonly string PrimaryValidDataPath;

    public readonly DirectoryInfo SecondaryValidSkyrimDir;
    public readonly string SecondaryValidDataPath;

    public readonly DirectoryInfo InvalidDir;

    public SkyrimPathResolverPriorityTests(ITestOutputHelper output)
    {
        _output = output;
        _mockRegistry = Substitute.For<IRegistry>();
        _mockEnvironment = Substitute.For<IRuntimeEnvironment>();

        // 1. Primary valid path
        var primaryRoot = Path.Combine(Path.GetTempPath(), "Skyrim SE");
        PrimaryValidSkyrimDir = new DirectoryInfo(primaryRoot);
        PrimaryValidDataPath = Path.Combine(primaryRoot, "Data");
        Directory.CreateDirectory(PrimaryValidDataPath);
        File.Create(Path.Combine(primaryRoot, "SkyrimSE.exe")).Close();

        // 2. Secondary valid path
        var secondaryRoot = Path.Combine(Path.GetTempPath(), "Skyrim");
        SecondaryValidSkyrimDir = new DirectoryInfo(secondaryRoot);
        SecondaryValidDataPath = Path.Combine(secondaryRoot, "Data");
        Directory.CreateDirectory(SecondaryValidDataPath);
        File.Create(Path.Combine(secondaryRoot, "SkyrimSELauncher.exe")).Close();

        // 3. Invalid path without .exe
        var invalidRoot = Path.Combine(Path.GetTempPath(), "Fallout");
        InvalidDir = new DirectoryInfo(Path.Combine(invalidRoot, "Data"));
        Directory.CreateDirectory(InvalidDir.FullName);
    }


    [Fact(DisplayName = "Priority 1: Command line arguments (--tesv)")]
    public void Resolve_Should_PrioritizeCommandLineArgs()
    {
        LaunchOptions.Parse(["--tesv", PrimaryValidSkyrimDir.FullName], caseInsensitive: true);
        _mockEnvironment.CurrentDirectory.Returns(SecondaryValidSkyrimDir.FullName);
        SetRegistryPath(InvalidDir.FullName);
        var sut = new SkyrimPathResolver(_mockEnvironment, _mockRegistry);

        _output.WriteLine($"Args: {PrimaryValidSkyrimDir.FullName}");
        _output.WriteLine($"Current Dir: {_mockEnvironment.CurrentDirectory}");
        _output.WriteLine($"Registry: {InvalidDir.FullName}");

        var result = sut.Resolve();

        _output.WriteLine($"\nResolved path: {result.FullName}");

        Assert.Equal(PrimaryValidDataPath, result.FullName);
    }

    [Fact(DisplayName = "Priority 2: Current directory (Start In)")]
    public void Resolve_Should_PrioritizeStartIn_When_ArgsAreMissing()
    {
        LaunchOptions.Current = null;
        _mockEnvironment.CurrentDirectory.Returns(PrimaryValidSkyrimDir.FullName);
        SetRegistryPath(SecondaryValidSkyrimDir.FullName);
        var sut = new SkyrimPathResolver(_mockEnvironment, _mockRegistry);

        _output.WriteLine("Args: <null>");
        _output.WriteLine($"Current Dir: {_mockEnvironment.CurrentDirectory}");
        _output.WriteLine($"Registry: {SecondaryValidSkyrimDir.FullName}");

        var result = sut.Resolve();

        _output.WriteLine($"\nResolved path: {result.FullName}");

        Assert.Equal(PrimaryValidDataPath, result.FullName);
    }

    [Fact(DisplayName = "Priority 3: Registry")]
    public void Resolve_Should_UseRegistry_When_ArgsAndCurrentDirAreInvalid()
    {
        LaunchOptions.Current = null;
        _mockEnvironment.CurrentDirectory.Returns(InvalidDir.FullName);
        SetRegistryPath(PrimaryValidSkyrimDir.FullName);
        var sut = new SkyrimPathResolver(_mockEnvironment, _mockRegistry);

        _output.WriteLine("Args: <null>");
        _output.WriteLine($"Current Dir: {_mockEnvironment.CurrentDirectory}");
        _output.WriteLine($"Registry: {PrimaryValidSkyrimDir.FullName}");

        var result = sut.Resolve();

        _output.WriteLine($"\nResolved path: {result.FullName}");

        Assert.Equal(PrimaryValidDataPath, result.FullName);
    }

    [Fact(DisplayName = "Extreme case: Nothing found")]
    public void Resolve_Should_ReturnCurrentDirectory_When_NoValidPathIsFound()
    {
        LaunchOptions.Current = null;
        _mockEnvironment.CurrentDirectory.Returns(InvalidDir.FullName);
        var sut = new SkyrimPathResolver(_mockEnvironment, registry: null);

        _output.WriteLine("Args: <null>");
        _output.WriteLine($"Current Dir set to: {_mockEnvironment.CurrentDirectory}");
        _output.WriteLine("Registry is empty.");

        var result = sut.Resolve();

        _output.WriteLine($"\nResolved path: {result.FullName}");

        Assert.Equal(InvalidDir.FullName, result.FullName);
    }

    private void SetRegistryPath(string path)
    {
        _mockRegistry.GetValue(Arg.Any<RegistryKey>(), SkyrimPathResolver.RegistrySubKey, SkyrimPathResolver.RegistryValueName).Returns(path);
    }

    public void Dispose()
    {
        LaunchOptions.Current = null;

        if (Directory.Exists(PrimaryValidSkyrimDir.FullName)) Directory.Delete(PrimaryValidSkyrimDir.FullName, true);
        if (Directory.Exists(SecondaryValidSkyrimDir.FullName)) Directory.Delete(SecondaryValidSkyrimDir.FullName, true);
        if (Directory.Exists(InvalidDir.FullName)) Directory.Delete(InvalidDir.FullName, true);
        
        GC.SuppressFinalize(this);
    }

    ~SkyrimPathResolverPriorityTests() => Dispose();
}