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
    private readonly IFileSystem _mockFileSystem;

    private static readonly string _primaryValidSkyrimRoot = Path.Combine(Path.GetTempPath(), "Skyrim SE");
    private static readonly string _primaryValidDataPath = Path.Combine(_primaryValidSkyrimRoot, "Data");
    private static readonly string _secondaryValidSkyrimRoot = Path.Combine(Path.GetTempPath(), "Skyrim");
    private static readonly string _secondaryValidDataPath = Path.Combine(_secondaryValidSkyrimRoot, "Data");
    private static readonly string _invalidRoot = Path.Combine(Path.GetTempPath(), "Fallout");
    private static readonly string _invalidDataPath = Path.Combine(_invalidRoot, "Data");

    public SkyrimPathResolverPriorityTests(ITestOutputHelper output)
    {
        _output = output;
        _mockRegistry = Substitute.For<IRegistry>();
        _mockEnvironment = Substitute.For<IRuntimeEnvironment>();
        _mockFileSystem = Substitute.For<IFileSystem>();

        _mockFileSystem.Combine(Arg.Any<string[]>()).Returns(callInfo => Path.Combine(callInfo.Arg<string[]>()));
        _mockFileSystem.GetFileName(Arg.Any<string>()).Returns(callInfo => Path.GetFileName(callInfo.Arg<string>()));

        // 1. Primary valid path
        SetupValidPath(_primaryValidSkyrimRoot, _primaryValidDataPath, hasExe: true, hasLauncher: false);

        // 2. Secondary valid path
        SetupValidPath(_secondaryValidSkyrimRoot, _secondaryValidDataPath, hasExe: false, hasLauncher: true);

        // 3. Invalid path without .exe
        SetupValidPath(_invalidRoot, _invalidDataPath, hasExe: false, hasLauncher: false);
    }

    private void SetupValidPath(string rootPath, string dataPath, bool hasExe, bool hasLauncher)
    {
        _mockFileSystem.DirectoryExists(dataPath).Returns(true);
        _mockFileSystem.GetParentDirectoryPath(dataPath).Returns(rootPath);
        _mockFileSystem.DirectoryExists(rootPath).Returns(true);
        _mockFileSystem.FileExists(Path.Combine(rootPath, "SkyrimSE.exe")).Returns(hasExe);
        _mockFileSystem.FileExists(Path.Combine(rootPath, "SkyrimSELauncher.exe")).Returns(hasLauncher);
    }


    public static TheoryData<string, string?, string, string?, string> TestScenarios = new()
    {
        {
            "Priority 1: Command line arguments (--tesv)",
            _primaryValidSkyrimRoot,                // Command line arg (top priority)
            _secondaryValidSkyrimRoot,              // Current directory
            _invalidRoot,                           // Path registry
            _primaryValidDataPath                   // Expected behavior
        },
        {
            "Priority 2: Current directory (Start In)",
            null,                                   // Command line arg (none)
            _primaryValidSkyrimRoot,                // Current directory (top priority)
            _secondaryValidSkyrimRoot,              // Path registry
            _primaryValidDataPath                   // Expected behavior
        },
        {
            "Priority 3: Registry",
            null,                                   // Command line args (none)
            _invalidRoot,                           // Current directory (invalid)
            _primaryValidSkyrimRoot,                // Path registry (top priority)
            _primaryValidDataPath                   // Expected behavior
        },
        {
            "Extreme case: Nothing found",
            null,                                   // Command line args (none)
            _invalidDataPath,                       // Current directory (invalid)
            null,                                   // Path registry (none)
            _invalidDataPath                        // Expected behavior: return Current Directory
        }
    };


    [Theory(DisplayName = "Resolve_ReturnCorrectPath_BasedOnPriority")]
    [MemberData(nameof(TestScenarios))]
    public void Resolve_ReturnCorrectPath_BasedOnPriority(
        string scenario, string? commandLinePath, string currentDirectory, string? registryPath, string expectedPath)
    {
        _output.WriteLine($"--- Running scenario: {scenario} ---");

        // 1. Setting command line arguments
        if (commandLinePath is not null)
        {
            LaunchOptions.Parse(["--tesv", commandLinePath], caseInsensitive: true);
            _output.WriteLine($"Args: {commandLinePath}");
        }
        else
        {
            LaunchOptions.Current = null;
            _output.WriteLine("Args: <null>");
        }

        // 2. Setting Current Directory
        _mockEnvironment.CurrentDirectory.Returns(currentDirectory);
        _output.WriteLine($"Current Dir: {currentDirectory}");

        // 3. Setting Path Registry
        IRegistry? registryToUse = null;
        if (registryPath is not null)
        {
            SetRegistryPath(registryPath);
            registryToUse = _mockRegistry;
            _output.WriteLine($"Registry: {registryPath}");
        }
        else
        {
            _output.WriteLine("Registry: <null>");
        }

        var sut = new SkyrimPathResolver(_mockEnvironment, registryToUse, _mockFileSystem);

        var result = sut.Resolve();
        _output.WriteLine($"\nResolved path: {result.FullName}");

        Assert.Equal(expectedPath, result.FullName);
    }

    private void SetRegistryPath(string path)
    {
        _mockRegistry.GetValue(Arg.Any<RegistryKey>(), SkyrimPathResolver.RegistrySubKey, SkyrimPathResolver.RegistryValueName).Returns(path);
    }

    public void Dispose()
    {
        LaunchOptions.Current = null;
        
        GC.SuppressFinalize(this);
    }
}