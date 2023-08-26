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

		private FileInfo LookupNestedFile(string name)
		{
			string[] sections = name.Split('~'); 

			var targetProject = projectMap[sections[0]];
			return targetProject.LookupFileHandle(sections[1]); 
		}
		public FileInfo LookupFile(string name)
		{
			name = name.ToLower();
			return name.Contains('~') ? LookupNestedFile(name) : fileProjectMap[name].LookupFileHandle(name);	
		}

		public bool ContainsFile(string name)
		{
			return name.Contains('~')
		}
		
	}
}
