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
		var activeModsFilePath = Path.Combine(Environment.CurrentDirectory, "Pandora_Engine", "ActiveMods.json");
		JsonModSettingsStore settingsStore = new JsonModSettingsStore(activeModsFilePath);
		ModService service = new ModService(settingsStore, modInfoProviders);
		var mods = await service.LoadModsAsync(BehaviourEngine.AssemblyDirectory, new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Data")));
		BehaviourEngine engine = new BehaviourEngine(new SkyrimDebugConfiguration());
		engine.SetOutputPath(Resources.OutputDirectory);
		await engine.PreloadAsync();
		Assert.True(await engine.LaunchAsync(mods.Select(m => m.ModInfo).ToList()));
		var skyrimPatcher = engine.Configuration.Patcher as SkyrimPatcher;
		Assert.NotNull(skyrimPatcher);

		var activePackFiles = skyrimPatcher.NemesisAssembler.ProjectManager.ActivePackFiles;
		foreach (var activePackFile in activePackFiles)
		{
			PackFileAssert.DowncastValidPackFile(activePackFile);
		}
	}

}
