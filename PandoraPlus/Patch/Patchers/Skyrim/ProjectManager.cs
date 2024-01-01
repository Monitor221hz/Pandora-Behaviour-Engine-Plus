using Pandora.Patch.Patchers.Skyrim.AnimData;
using Pandora.Patch.Patchers.Skyrim.AnimSetData;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pandora.Core.Patchers.Skyrim
{
	public class ProjectManager
	{

		private Dictionary<string, Project> projectMap { get; set; } = new Dictionary<string, Project>();

		private Dictionary<string, Project> fileProjectMap { get; set; } = new Dictionary<string, Project>(); 

		private DirectoryInfo templateFolder { get; set; }

		private DirectoryInfo outputFolder { get; set; }


		private PackFileCache packFileCache { get; set; } = new PackFileCache();
		public  HashSet<PackFile> ActivePackFiles { get; private set;  } =  new HashSet<PackFile>();






		public ProjectManager(DirectoryInfo templateFolder, DirectoryInfo outputFolder)
        {
            this.templateFolder = templateFolder;
			this.outputFolder = outputFolder;


        }

		public void GetAnimationInfo(StringBuilder builder)
		{
			var projects = projectMap.Values;
			uint totalAnimationCount = 0;

			builder.AppendLine();
			foreach(var project in projects)
			{
				var animCount = project.CharacterFile.NewAnimationCount; 
				if (animCount == 0) { continue; }
				totalAnimationCount+= animCount;	
				builder.AppendLine($"{animCount} animations added to {project.Identifier}.");
			}
			builder.AppendLine();
			builder.AppendLine($"{totalAnimationCount} total animations added.");
		}

		public bool TryGetProject(string name, out Project? project) => projectMap.TryGetValue(name, out project);
		public bool ProjectExists(string name) => projectMap.ContainsKey(name);



		
		public async Task LoadTrackedProjectsAsync()
		{
			FileInfo projectList = new FileInfo($"{templateFolder.FullName}\\vanilla_projectpaths.txt");
			List<Task> projectLoadTasks = new List<Task>();
			string? expectedLine = null;
			List<string> projectPaths = new List<string>();
			using (var readStream = projectList.OpenRead())
			{
				using (var streamReader = new StreamReader(readStream))
				{
					while ((expectedLine = streamReader.ReadLine()) != null)
					{
						if (String.IsNullOrWhiteSpace(expectedLine)) continue;
						projectPaths.Add(expectedLine);
					}
				}
			}

			Parallel.ForEach(projectPaths, projectFilePath => { LoadProject(projectFilePath); });
		}
		public void LoadTrackedProjects()
		{
			FileInfo projectList = new FileInfo($"{templateFolder.FullName}\\vanilla_projectpaths.txt");
			string? expectedLine = null;
			List<string> projectPaths = new List<string>();
			using (var readStream = projectList.OpenRead())
			{
				using (var streamReader = new StreamReader(readStream))
				{
					while ((expectedLine = streamReader.ReadLine()) != null)
					{
						if (String.IsNullOrWhiteSpace(expectedLine)) continue;
						projectPaths.Add(expectedLine);

					}
				}
			}
			foreach (var projectPath in projectPaths)
			{
				LoadProject(projectPath);
			}

		}
		public void ExtractProjects()
		{
			Parallel.ForEach(projectMap.Values, project => { ExtractProject(project); });
		}
		public async Task LoadProjectAsync(string projectFilePath)
		{
			if (String.IsNullOrWhiteSpace(projectFilePath)) return;


				var project = Project.Load(new FileInfo(Path.Join(templateFolder.FullName, projectFilePath)), packFileCache);

				lock (projectMap) projectMap.Add(project.Identifier, project);


				await Task.Run(() => { ExtractProject(project); });
				//ExtractProject(project);

		}
		public void LoadProject(string projectFilePath)
		{
			if (String.IsNullOrWhiteSpace(projectFilePath)) return;

			lock (projectMap)
			{
				var project = Project.Load(new FileInfo(Path.Join(templateFolder.FullName, projectFilePath)), packFileCache);

				projectMap.Add(project.Identifier, project);



			    lock (project) ExtractProject(project);
			}

		}

		private void ExtractProject(Project project)
		{
			lock (fileProjectMap)
			{
				List<string> fileNames = project.MapFiles(packFileCache);
				foreach (string file in fileNames)
				{
					if (!fileProjectMap.ContainsKey(file)) fileProjectMap.Add(file, project);
				}
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

		public bool ProjectLoaded(string name) => projectMap.ContainsKey(name);
		public Project LookupProject(string name) => projectMap[name];

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
				packFile.Activate();
			}
			return packFile;
		}
		public PackFile ActivatePackFile(PackFile packFile)
		{
			lock (ActivePackFiles)
				lock (packFile)
				{
					if (ActivePackFiles.Contains(packFile)) return packFile;
					ActivePackFiles.Add(packFile);
					packFile.Activate();
				}
			return packFile;
		}

		public void ApplyPatches()
		{
			packFileCache.DeletePackFileOutput();

			Parallel.ForEach(ActivePackFiles, packFile =>
			{
				packFile.ApplyChanges();
				//packFile.Map.Save(Path.Join(Directory.GetCurrentDirectory(), packFile.InputHandle.Name));
				packFile.Export();
			});
			//foreach (PackFile packFile in ActivePackFiles)
			//{
			//	packFile.ApplyChanges();
			//	packFile.Map.Save(Path.Join(Directory.GetCurrentDirectory(), packFile.InputHandle.Name));
			//	packFile.Export();
			//}

		}

		public void ApplyPatchesParallel()
		{

			packFileCache.DeletePackFileOutput();

			Parallel.ForEach(ActivePackFiles, packFile =>
			{
				packFile.ApplyChanges();
			});

//#if DEBUG || DEBUGRELEASE
//			foreach(PackFile packFile in ActivePackFiles) { Debug.WriteLine(packFile.UniqueName);  }
//#endif
			Parallel.ForEach(ActivePackFiles, packFile => { packFile.Export(); });

		}
	}
}
