using Pandora.Models;

namespace PandoraTests;
public static class Resources
{
	public static readonly DirectoryInfo TemplateDirectory = new(Path.Combine(BehaviourEngine.AssemblyDirectory.FullName, "Pandora_Engine", "Skyrim", "Template"));
	public static readonly DirectoryInfo OutputDirectory = new(Path.Combine(Directory.GetCurrentDirectory(), "Output"));
	public static readonly DirectoryInfo CurrentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

	static Resources()
	{
		OutputDirectory.Create();
		TemplateDirectory.Create();
	}
}
