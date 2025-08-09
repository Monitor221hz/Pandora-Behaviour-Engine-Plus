using Pandora.Data;
using Pandora.Models;
using Pandora.Models.Patch.Configs;
using Pandora.Models.Patch.Skyrim64;
using Pandora.Services;

namespace PandoraTests;

public class EndToEndTests
{
	[Fact]
	public async Task EngineOutputTest()
	{
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
		BehaviourEngine engine = new BehaviourEngine().SetOutputPath(Resources.OutputDirectory).SetConfiguration(new SkyrimDebugConfiguration());
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
