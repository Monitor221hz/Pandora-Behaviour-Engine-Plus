// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pandora.API.Data;
using Pandora.API.Patch.Config;
using Pandora.API.Patch.IOManagers;
using Pandora.API.Patch.Skyrim64;
using Pandora.API.Services;
using Pandora.Data;
using Pandora.Models.Patch.Configs;
using Pandora.Models.Patch.IO.Skyrim64;
using Pandora.Services;
using PandoraTests.Utils;

namespace PandoraTests;

public sealed class DependencyInjectedCluster : IDisposable
{
    public IServiceProvider ServiceProvider { get; }

    public DependencyInjectedCluster()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddPandoraServices();
        serviceCollection.AddSingleton<IPathResolver, TestPathResolver>();

        serviceCollection.RemoveAll<IMetaDataExporter<IPackFile>>();

        serviceCollection.AddSingleton<DebugPackFileExporter>();

        serviceCollection.AddSingleton<TestCapturingExporter>(sp =>
            new TestCapturingExporter(sp.GetRequiredService<DebugPackFileExporter>()));

        serviceCollection.AddSingleton<IMetaDataExporter<IPackFile>>(sp =>
            sp.GetRequiredService<TestCapturingExporter>());

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public void Dispose() { }
}

public class EndToEndTests : IClassFixture<DependencyInjectedCluster>
{
    private readonly ITestOutputHelper _output;
    private readonly IServiceProvider _serviceProvider;

    public EndToEndTests(DependencyInjectedCluster cluster, ITestOutputHelper output)
    {
        _serviceProvider = cluster.ServiceProvider;
        _output = output;
    }

    [Fact]
    public async Task EngineOutputTest()
    {
        var pathResolver = _serviceProvider.GetRequiredService<IPathResolver>();
        _output.WriteLine(
            $"Starting EngineOutputTest. OutputDirectory: {Resources.OutputDirectory.FullName}"
        );
        var modLoader = _serviceProvider.GetRequiredService<IModLoaderService>();
        var configurationFactory = _serviceProvider.GetRequiredService<
            IEngineConfigurationFactory<SkyrimDebugConfiguration>
        >();

        // Loading mods
        var modInfoProviders = new List<IModInfoProvider>
        {
            new NemesisModInfoProvider(),
            new PandoraModInfoProvider(),
        };
        var directories = new List<DirectoryInfo>
        {
            pathResolver.GetAssemblyFolder(),
            new(Path.Combine(pathResolver.GetCurrentFolder().FullName, "Data")),
        };
        var activeModsFilePath = pathResolver.GetActiveModsFile();
        _output.WriteLine(
            $"Loading mods from directories: {string.Join(", ", directories.Select(d => d.FullName))}"
        );
        var mods = await modLoader.LoadModsAsync(modInfoProviders, directories);
        _output.WriteLine($"Loaded {mods.Count} mods");

        Assert.NotEmpty(mods);

        // Initialization Engine and Launch
        var engine = _serviceProvider.GetRequiredService<IBehaviourEngine>();
        await engine.SetConfigurationAsync(configurationFactory);
        _output.WriteLine(
            $"BehaviourEngine configured with OutputPath: {Resources.OutputDirectory.FullName}"
        );

        await engine.InitializeAsync();
        _output.WriteLine("PreloadAsync completed");

        var result = await engine.LaunchAsync(mods.ToList());
        _output.WriteLine($"Launch Result: {result.Message}");
        Assert.True(result.Success, $"LaunchAsync failed: {result.Message}");
        _output.WriteLine("LaunchAsync completed successfully");

        //var skyrimPatcher = engine.Configuration.Patcher as SkyrimPatcher;
        //Assert.NotNull(skyrimPatcher);
        //_output.WriteLine("SkyrimPatcher retrieved");

        //var activePackFiles = skyrimPatcher.NemesisAssembler.ProjectManager.ActivePackFiles;
        //_output.WriteLine($"Found {activePackFiles.Count} ActivePackFiles");

        //Assert.NotEmpty(activePackFiles);

        //foreach (var activePackFile in activePackFiles)
        //{
        //    _output.WriteLine(
        //        $"PackFile: Name={activePackFile.Name}, InputHandle={activePackFile.InputHandle.FullName}, OutputHandle={activePackFile.OutputHandle.FullName}, RelativeOutputDirectoryPath={activePackFile.RelativeOutputDirectoryPath}"
        //    );
        //    PackFileAssert.DowncastValidPackFile(activePackFile);
        //}

        var spyExporter = _serviceProvider.GetRequiredService<TestCapturingExporter>();
        Assert.NotNull(spyExporter);
        var activePackFiles = spyExporter.CapturedPackFiles;
        _output.WriteLine($"Spy captured {activePackFiles.Count} ActivePackFiles");

        Assert.NotEmpty(activePackFiles);

        foreach (var activePackFile in activePackFiles)
        {
            _output.WriteLine(
                $"PackFile: Name={activePackFile.Name}, InputHandle={activePackFile.InputHandle.FullName}, OutputHandle={activePackFile.OutputHandle.FullName}, RelativeOutputDirectoryPath={activePackFile.RelativeOutputDirectoryPath}"
            );
            PackFileAssert.DowncastValidPackFile(activePackFile);
        }

        // Checking for meshes with .hkx files
        var meshesPath = pathResolver.GetOutputMeshFolder().FullName;
        Assert.True(Directory.Exists(meshesPath), $"Meshes directory does NOT exist: {meshesPath}");
        _output.WriteLine($"Meshes directory exists: {meshesPath}");

        var meshFiles = Directory.GetFiles(meshesPath, "*.hkx", SearchOption.AllDirectories);
        _output.WriteLine($"Found {meshFiles.Length} mesh files");
        Assert.NotEmpty(meshFiles);

        foreach (var file in meshFiles)
        {
            _output.WriteLine($"Found mesh file: {file}");
        }
    }
}
