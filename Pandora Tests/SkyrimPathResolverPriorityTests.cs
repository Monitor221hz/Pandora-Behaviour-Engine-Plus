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

    private readonly DirectoryInfo _primaryValidSkyrimDir;
    private readonly string _primaryValidDataPath;

    private readonly DirectoryInfo _secondaryValidSkyrimDir;

    private readonly DirectoryInfo _invalidDir;

    public SkyrimPathResolverPriorityTests(ITestOutputHelper output)
    {
        _output = output;
        _mockRegistry = Substitute.For<IRegistry>();
        _mockEnvironment = Substitute.For<IRuntimeEnvironment>();

        // 1. Primary valid path
        var primaryRoot = Path.Combine(Path.GetTempPath(), "Skyrim SE");
        _primaryValidSkyrimDir = new DirectoryInfo(primaryRoot);
        _primaryValidDataPath = Path.Combine(primaryRoot, "Data");
        Directory.CreateDirectory(_primaryValidDataPath);
        File.Create(Path.Combine(primaryRoot, "SkyrimSE.exe")).Close();

        // 2. Secondary valid path
        var secondaryRoot = Path.Combine(Path.GetTempPath(), "Skyrim");
        _secondaryValidSkyrimDir = new DirectoryInfo(secondaryRoot);
        var secondaryValidDataPath = Path.Combine(secondaryRoot, "Data");
        Directory.CreateDirectory(secondaryValidDataPath);
        File.Create(Path.Combine(secondaryRoot, "SkyrimSELauncher.exe")).Close();

        // 3. Invalid path without .exe
        var invalidRoot = Path.Combine(Path.GetTempPath(), "Fallout");
        _invalidDir = new DirectoryInfo(Path.Combine(invalidRoot, "Data"));
        Directory.CreateDirectory(_invalidDir.FullName);
    }


    [Fact(DisplayName = "Priority 1: Command line arguments (--tesv)")]
    public void Resolve_Should_PrioritizeCommandLineArgs()
    {
        LaunchOptions.Parse(["--tesv", _primaryValidSkyrimDir.FullName], caseInsensitive: true);
        _mockEnvironment.CurrentDirectory.Returns(_secondaryValidSkyrimDir.FullName);
        SetRegistryPath(_invalidDir.FullName);
        var sut = new SkyrimPathResolver(_mockEnvironment, _mockRegistry);

        _output.WriteLine($"Args: {_primaryValidSkyrimDir.FullName}");
        _output.WriteLine($"Current Dir: {_mockEnvironment.CurrentDirectory}");
        _output.WriteLine($"Registry: {_invalidDir.FullName}");

        var result = sut.Resolve();

        _output.WriteLine($"\nResolved path: {result.FullName}");

        Assert.Equal(_primaryValidDataPath, result.FullName);
    }

    [Fact(DisplayName = "Priority 2: Current directory (Start In)")]
    public void Resolve_Should_PrioritizeStartIn_When_ArgsAreMissing()
    {
        LaunchOptions.Current = null;
        _mockEnvironment.CurrentDirectory.Returns(_primaryValidSkyrimDir.FullName);
        SetRegistryPath(_secondaryValidSkyrimDir.FullName);
        var sut = new SkyrimPathResolver(_mockEnvironment, _mockRegistry);

        _output.WriteLine("Args: <null>");
        _output.WriteLine($"Current Dir: {_mockEnvironment.CurrentDirectory}");
        _output.WriteLine($"Registry: {_secondaryValidSkyrimDir.FullName}");

        var result = sut.Resolve();

        _output.WriteLine($"\nResolved path: {result.FullName}");

        Assert.Equal(_primaryValidDataPath, result.FullName);
    }

    [Fact(DisplayName = "Priority 3: Registry")]
    public void Resolve_Should_UseRegistry_When_ArgsAndCurrentDirAreInvalid()
    {
        LaunchOptions.Current = null;
        _mockEnvironment.CurrentDirectory.Returns(_invalidDir.FullName);
        SetRegistryPath(_primaryValidSkyrimDir.FullName);
        var sut = new SkyrimPathResolver(_mockEnvironment, _mockRegistry);

        _output.WriteLine("Args: <null>");
        _output.WriteLine($"Current Dir: {_mockEnvironment.CurrentDirectory}");
        _output.WriteLine($"Registry: {_primaryValidSkyrimDir.FullName}");

        var result = sut.Resolve();

        _output.WriteLine($"\nResolved path: {result.FullName}");

        Assert.Equal(_primaryValidDataPath, result.FullName);
    }

    [Fact(DisplayName = "Extreme case: Nothing found")]
    public void Resolve_Should_ReturnCurrentDirectory_When_NoValidPathIsFound()
    {
        LaunchOptions.Current = null;
        _mockEnvironment.CurrentDirectory.Returns(_invalidDir.FullName);
        var sut = new SkyrimPathResolver(_mockEnvironment, registry: null);

        _output.WriteLine("Args: <null>");
        _output.WriteLine($"Current Dir set to: {_mockEnvironment.CurrentDirectory}");
        _output.WriteLine("Registry is empty.");

        var result = sut.Resolve();

        _output.WriteLine($"\nResolved path: {result.FullName}");

        Assert.Equal(_invalidDir.FullName, result.FullName);
    }

    private void SetRegistryPath(string path)
    {
        _mockRegistry.GetValue(Arg.Any<RegistryKey>(), SkyrimPathResolver.RegistrySubKey, SkyrimPathResolver.RegistryValueName).Returns(path);
    }

    public void Dispose()
    {
        LaunchOptions.Current = null;

        if (Directory.Exists(_primaryValidSkyrimDir.FullName)) Directory.Delete(_primaryValidSkyrimDir.FullName, true);
        if (Directory.Exists(_secondaryValidSkyrimDir.FullName)) Directory.Delete(_secondaryValidSkyrimDir.FullName, true);
        if (Directory.Exists(_invalidDir.FullName)) Directory.Delete(_invalidDir.FullName, true);
        
        GC.SuppressFinalize(this);
    }
}