using Pandora.Models;
using Pandora.Models.Patch.Configs;
using Pandora.Models.Patch.Skyrim64;
using Pandora.Services;

namespace PandoraTests;

public class PatchTests
{
	[Fact]
	public async void NemesisTest()
	{
		ModService service = new ModService(Path.Combine(BehaviourEngine.AssemblyDirectory.FullName, "Pandora_Engine", "ActiveMods.txt"));
		var mods = await service.LoadModsAsync(BehaviourEngine.AssemblyDirectory, new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Data")));
		BehaviourEngine engine = new BehaviourEngine(new SkyrimDebugConfiguration());
		DirectoryInfo outputDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Output"));
		outputDirectory.Create();
		engine.SetOutputPath(outputDirectory);
		await engine.PreloadAsync();
		Assert.True(await engine.LaunchAsync(mods));
		var skyrimPatcher = engine.Configuration.Patcher as SkyrimPatcher;
		Assert.NotNull(skyrimPatcher);

		var activePackFiles = skyrimPatcher.NemesisAssembler.ProjectManager.ActivePackFiles;
		foreach (var activePackFile in activePackFiles)
		{
			PackFileAssert.DowncastValidPackFile(activePackFile);
		}
	}
}
