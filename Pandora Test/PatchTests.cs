using Pandora.Models;
using Pandora.Services;

namespace PandoraTests;

public class PatchTests
{
	[Fact]
	public async void NemesisTest()
	{
		ModService service = new ModService(Path.Combine(BehaviourEngine.AssemblyDirectory.FullName, "Pandora_Engine", "ActiveMods.txt"));
		var mods = await service.LoadModsAsync(BehaviourEngine.AssemblyDirectory, new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Data")));
		BehaviourEngine engine = new BehaviourEngine();
		DirectoryInfo outputDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Output"));
		outputDirectory.Create();
		engine.SetOutputPath(outputDirectory);
		await engine.PreloadAsync();
		await engine.LaunchAsync(mods);
	}
}
