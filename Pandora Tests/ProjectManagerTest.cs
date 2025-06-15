using Pandora.Models;
using Pandora.Models.Patch.Skyrim64;

namespace PandoraTests;

public class ProjectManagerTest
{
	private static readonly DirectoryInfo templateFolder = new DirectoryInfo(Path.Combine(BehaviourEngine.AssemblyDirectory.FullName, "Pandora_Engine", "Skyrim", "Template"));
	private static readonly DirectoryInfo currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
	[Fact]
	public void ProjectLoadTest()
	{
		ProjectManager manager = new ProjectManager(templateFolder, currentDirectory);
		manager.LoadTrackedProjects();
	}
}