using Pandora.Data;
using Pandora.Models;
using Pandora.Models.Patch.Configs;
using Pandora.Models.Patch.Skyrim64;
using Pandora.Services;

namespace PandoraTests;

public class EndToEndTests
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	[Fact]
	public async Task EngineOutputTest()
	{
        logger.Info($"Starting EngineOutputTest. OutputDirectory: {Resources.OutputDirectory.FullName}");

        // Loading mods
		var modInfoProviders = new List<IModInfoProvider>
		{
			new NemesisModInfoProvider(),
			new PandoraModInfoProvider()
		};
		var directories = new List<DirectoryInfo>
		{
            BehaviourEngine.AssemblyDirectory, 
			new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Data"))

        };
		var activeModsFilePath = Path.Combine(Environment.CurrentDirectory, "Pandora_Engine", "ActiveMods.json");
		var mods = await ModLoader.LoadModsAsync(modInfoProviders, directories);
        logger.Info($"Loaded {mods.Count} mods");
        // Initialization Engine and Launch
        BehaviourEngine engine = new BehaviourEngine()
            .SetConfiguration(new SkyrimDebugConfiguration())
            .SetOutputPath(Resources.OutputDirectory); // SetOutputPath() must be after configuration initialization
		await engine.PreloadAsync();
		Assert.True(await engine.LaunchAsync(mods.ToList()));
		var skyrimPatcher = engine.Configuration.Patcher as SkyrimPatcher;
		Assert.NotNull(skyrimPatcher);

		var activePackFiles = skyrimPatcher.NemesisAssembler.ProjectManager.ActivePackFiles;
		foreach (var activePackFile in activePackFiles)
		{
			PackFileAssert.DowncastValidPackFile(activePackFile);
		}
	}

}
