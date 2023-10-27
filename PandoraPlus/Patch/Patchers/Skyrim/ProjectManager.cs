using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Core.Patchers.Skyrim
{
	public class ProjectManager
	{

		private Dictionary<string, Project> projectMap { get; set; } = new Dictionary<string, Project>();

		private Dictionary<string, Project> fileProjectMap { get; set; } = new Dictionary<string, Project>(); 

		public DirectoryInfo TemplateFolder = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Pandora_Engine\\Skyrim\\Template");

		public  HashSet<PackFile> ActivePackFiles { get; private set;  } =  new HashSet<PackFile>();

		
		public void LoadProject(string projectFilePath)
		{
			var project = Project.Load(Path.Join(TemplateFolder.FullName, projectFilePath));

			projectMap.Add(project.Identifier, project); 

			ExtractProject(project);
		}

		private void ExtractProject(Project project)
		{
			List<string> fileNames = project.MapFiles(); 
			foreach(string file in fileNames)
			{
				if (!fileProjectMap.ContainsKey(file)) fileProjectMap.Add(file, project);
			}
		}
		
		private PackFile LookupNestedPackFile(string name)
		{
			string[] sections = name.Split('~'); 

			var targetProject = projectMap[sections[0]];
			return targetProject.LookupPackFile(sections[1]); 
		}

		private bool ContainsNestedPackFile(string name)
		{
			string[] sections = name.Split('~');

			Project targetProject;

			if (!projectMap.TryGetValue(sections[0], out targetProject!)) return false; 

			return targetProject.ContainsPackFile(sections[1]);
		}

		public bool ProjectLoaded(string name) => fileProjectMap.ContainsKey(name);
		public Project LookupProject(string name) => fileProjectMap[name];

		public PackFile LookupPackFile(string name)
		{
			name = name.ToLower();
			return name.Contains('~') ? LookupNestedPackFile(name) : fileProjectMap[name].LookupPackFile(name);	
		}

		public bool ContainsPackFile(string name)
		{
			return name.Contains('~') ? ContainsNestedPackFile(name) : fileProjectMap.ContainsKey(name) && fileProjectMap[name].ContainsPackFile(name);
		}

		public PackFile ActivatePackFile(string name)
		{
			
			PackFile packFile = LookupPackFile(name);
			lock(ActivePackFiles) 
			lock(packFile)
			{
				if (ActivePackFiles.Contains(packFile)) return packFile;
				ActivePackFiles.Add(packFile);
				packFile.Map.MapLayer(PackFile.ROOT_CONTAINER_NAME, true);
			}
			return packFile;
		}
		
	}
}
