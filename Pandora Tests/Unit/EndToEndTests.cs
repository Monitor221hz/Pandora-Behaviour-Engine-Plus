// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Pandora.API.Patch.Config;
using Pandora.Models.Engine;
using Pandora.Models.Patch.Configs;
using Pandora.Mods.Abstractions;
using Pandora.Paths;
using Pandora.Paths.Abstractions;
using PandoraTests.Utils;

namespace PandoraTests.Unit;

public sealed class DependencyInjectedCluster : IDisposable
{
    public IServiceProvider ServiceProvider { get; }

    public DependencyInjectedCluster()
    {
        var services = new ServiceCollection();

        var root = new DirectoryInfo(Environment.CurrentDirectory);
        root.Create();
        
        var gameDataDir = new DirectoryInfo(Path.Combine(root.FullName, "Data"));
        var outputDir = new DirectoryInfo(Path.Combine(root.FullName, "Output"));

        gameDataDir.Create();
        outputDir.Create();

        var pandoraEngineDir = outputDir.CreateSubdirectory("Pandora_Engine");
        var meshesDir = outputDir.CreateSubdirectory("meshes");

        var appEngineDir = root.CreateSubdirectory("Pandora_Engine");
        var templateDir = appEngineDir.CreateSubdirectory(Path.Combine("Skyrim", "Template"));

        var userPathsSub = Substitute.For<IUserPaths>();

        userPathsSub.Output.Returns(outputDir);
        userPathsSub.GameData.Returns(gameDataDir);

        services.AddSingleton(userPathsSub);

        var appPathsSub = Substitute.For<IApplicationPaths>();
        var assemblyDir = root;

        appPathsSub.AssemblyDirectory.Returns(assemblyDir);
        appPathsSub.EngineDirectory.Returns(appEngineDir);
        appPathsSub.TemplateDirectory.Returns(templateDir);
        appPathsSub.PathConfig.Returns(new FileInfo(Path.Combine(appEngineDir.FullName, "Paths.json")));

        services.AddSingleton(appPathsSub);

        var outputPathsSub = Substitute.For<IOutputPaths>();

        outputPathsSub.PandoraEngineDirectory.Returns(pandoraEngineDir);
        outputPathsSub.MeshesDirectory.Returns(meshesDir);
        outputPathsSub.PreviousOutputFile.Returns(new FileInfo(Path.Combine(pandoraEngineDir.FullName, "PreviousOutput.txt")));
        outputPathsSub.ActiveModsFile.Returns(new FileInfo(Path.Combine(pandoraEngineDir.FullName, "ActiveMods.json")));

        services.AddSingleton(outputPathsSub);

        services.AddSingleton<IEnginePathsFacade, EnginePathsFacade>();
        services.AddPandoraTestServices();

        ServiceProvider = services.BuildServiceProvider();
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
        var userPathContext = _serviceProvider.GetRequiredService<IUserPaths>();
        var appPathContext = _serviceProvider.GetRequiredService<IApplicationPaths>();
        var outputPathContext = _serviceProvider.GetRequiredService<IOutputPaths>();

        _output.WriteLine($"GameData: {userPathContext.GameData.FullName}");
        _output.WriteLine($"Output: {userPathContext.Output.FullName}");

        var modLoader = _serviceProvider.GetRequiredService<IModLoaderService>();
        var configurationFactory =
            _serviceProvider.GetRequiredService<
                IEngineConfigurationFactory<SkyrimDebugConfiguration>
            >();

        // Loading mods
        var mods = await modLoader.LoadModsAsync(
            [
                appPathContext.AssemblyDirectory,
                userPathContext.GameData
            ]
        );
        _output.WriteLine($"Loaded {mods.Count} mods");
        Assert.NotEmpty(mods);

        // Initialization Engine and Launch
        var engine = _serviceProvider.GetRequiredService<IBehaviourEngine>();
        await engine.SwitchConfigurationAsync(configurationFactory.Create());
        _output.WriteLine(
            $"BehaviourEngine configured with OutputPath: {userPathContext.Output.FullName}"
        );

        await engine.InitializeAsync();
        _output.WriteLine("PreloadAsync completed");

        var result = await engine.RunAsync(mods.ToList());
        _output.WriteLine($"Launch Result: {result.Message}");
        Assert.True(result.IsSuccess, $"LaunchAsync failed: {result.Message}");
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
        var meshesPath = outputPathContext.MeshesDirectory.FullName;
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
