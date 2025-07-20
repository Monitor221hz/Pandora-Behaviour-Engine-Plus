using Pandora.Models;

namespace PandoraTests;

public static class Resources
{
	public static readonly DirectoryInfo TemplateDirectory = new(Path.Combine(Environment.CurrentDirectory, "Pandora_Engine", "Skyrim", "Template"));
	public static readonly DirectoryInfo OutputDirectory = new(Path.Combine(Environment.CurrentDirectory, "Output"));
	public static readonly DirectoryInfo CurrentDirectory = new DirectoryInfo(Environment.CurrentDirectory);

	static Resources()
	{
		OutputDirectory.Create();
		TemplateDirectory.Create();
	}
}
