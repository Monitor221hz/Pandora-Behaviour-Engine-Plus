using Pandora.API.Patch.IOManagers;
using Pandora.Data;
using Pandora.Models;
using Pandora.Models.Patch.Configs;
using Pandora.Models.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using Pandora.Services;
using Pandora.Utils;

namespace PandoraTests;

public class EndToEndTests
{
    private readonly ITestOutputHelper _output;

    public EndToEndTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task EngineOutputTest()
    {
        _output.WriteLine($"Starting EngineOutputTest. OutputDirectory: {Resources.OutputDirectory.FullName}");

        // Loading mods
        var modInfoProviders = new List<IModInfoProvider>
        {
            new NemesisModInfoProvider(),
            new PandoraModInfoProvider()
        };
        var directories = new List<DirectoryInfo>
        {
            BehaviourEngine.AssemblyDirectory,
            new(Path.Combine(Environment.CurrentDirectory, "Data"))
        };
        var activeModsFilePath = PandoraPaths.ActiveModsFile;
        _output.WriteLine($"Loading mods from directories: {string.Join(", ", directories.Select(d => d.FullName))}");
        var mods = await ModLoader.LoadModsAsync(modInfoProviders, directories);
        _output.WriteLine($"Loaded {mods.Count} mods");

        Assert.NotEmpty(mods);


        // Initialization Engine and Launch
        BehaviourEngine engine = new BehaviourEngine()
            .SetConfiguration(new SkyrimDebugConfiguration())
            .SetOutputPath(Resources.OutputDirectory); // SetOutputPath() must be after configuration initialization
        _output.WriteLine($"BehaviourEngine configured with OutputPath: {Resources.OutputDirectory.FullName}");

        await engine.PreloadAsync();
        _output.WriteLine("PreloadAsync completed");

        bool launchSuccess = await engine.LaunchAsync(mods.ToList());
        Assert.True(launchSuccess, "LaunchAsync failed");
        _output.WriteLine("LaunchAsync completed successfully");

        var skyrimPatcher = engine.Configuration.Patcher as SkyrimPatcher;
        Assert.NotNull(skyrimPatcher);
        _output.WriteLine("SkyrimPatcher retrieved");

        var activePackFiles = skyrimPatcher.NemesisAssembler.ProjectManager.ActivePackFiles;
        _output.WriteLine($"Found {activePackFiles.Count} ActivePackFiles");

        Assert.NotEmpty(activePackFiles);

        foreach (var activePackFile in activePackFiles)
        {
            _output.WriteLine($"PackFile: Name={activePackFile.Name}, InputHandle={activePackFile.InputHandle.FullName}, OutputHandle={activePackFile.OutputHandle.FullName}, RelativeOutputDirectoryPath={activePackFile.RelativeOutputDirectoryPath}");
            PackFileAssert.DowncastValidPackFile(activePackFile);
        }


        // Checking for meshes with .hkx files
        var meshesPath = Path.Combine(Resources.OutputDirectory.FullName, "meshes");
        Assert.True(Directory.Exists(meshesPath), $"Meshes directory does NOT exist: {meshesPath}");
        _output.WriteLine($"Meshes directory exists: {meshesPath}");

        var meshFiles = Directory.GetFiles(meshesPath, "*.hkx", SearchOption.AllDirectories);
        _output.WriteLine($"Found {meshFiles.Length} mesh files");
        Assert.NotEmpty(meshFiles);

        foreach (var file in meshFiles)
        {
            _output.WriteLine($"Found mesh file: {file}");
        }


        // Checking for the presence of an exporter
        var exporterField = typeof(SkyrimPatcher).GetField("exporter", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (exporterField?.GetValue(skyrimPatcher) is IMetaDataExporter<PackFile> exporter)
        {
            Assert.NotNull(exporter.ExportDirectory);
            _output.WriteLine($"Exporter ExportDirectory: {exporter.ExportDirectory.FullName}");
        }
        else
        {
            Assert.Fail("Exporter is null or inaccessible");
        }
    }
}
