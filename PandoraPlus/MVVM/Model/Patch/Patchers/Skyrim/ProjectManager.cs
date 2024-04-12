using HKX2;
using NLog;
using Pandora.Patch.Patchers.Skyrim.AnimData;
using Pandora.Patch.Patchers.Skyrim.AnimSetData;
using Pandora.Patch.Patchers.Skyrim.FNIS;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pandora.Core.Patchers.Skyrim
{
	public class ProjectManager
	{
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		private Dictionary<string, Project> projectMap { get; set; } = new Dictionary<string, Project>();
		private Dictionary<string, Project> fileProjectMap { get; set; } = new Dictionary<string, Project>(); 

		private Dictionary<string, Project> folderProjectMap { get; set;} = new Dictionary<string, Project>();

		private Dictionary<string, List<Project>> linkedProjectMap { get; set; } = new Dictionary<string, List<Project>>();

		private DirectoryInfo templateFolder { get; set; }

		private DirectoryInfo outputFolder { get; set; }


		private PackFileCache packFileCache { get; set; } = new PackFileCache();
		public  HashSet<PackFile> ActivePackFiles { get; private set;  } =  new HashSet<PackFile>();

		private FNISParser fnisParser;

		private bool CompleteExportSuccess = true;


		public ProjectManager(DirectoryInfo templateFolder, DirectoryInfo outputFolder)
        {
            this.templateFolder = templateFolder;
			this.outputFolder = outputFolder;
			fnisParser = new FNISParser(this);


        }
		public void GetExportInfo(StringBuilder builder)
		{
			if (CompleteExportSuccess) { return; }
			builder.AppendLine();

			foreach(var failedPackFile in ActivePackFiles.Where(pf => !pf.ExportSuccess))
			{
				builder.AppendLine($"FATAL ERROR: Could not export {failedPackFile.UniqueName}. Check Engine.log for more information.");
			}
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

		public void GetFNISInfo(StringBuilder builder)
		{
			uint fnisModCount = 0;
			builder.AppendLine();
			foreach(IModInfo modInfo in fnisParser.ModInfos)
			{
				fnisModCount++;
				builder.AppendLine($"FNIS Mod {fnisModCount} : {modInfo.Name}");
				Logger.Info($"FNIS Mod {fnisModCount} : {modInfo.Name}");
			}
		}

		public bool TryGetProject(string name, out Project? project) => projectMap.TryGetValue(name, out project);
		public bool ProjectExists(string name) => projectMap.ContainsKey(name);



		

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
			LoadProjects(projectPaths);
		}
		public void LoadProjects(List<string> projectPaths)
		{
			foreach (var projectPath in projectPaths)
			{
				var project = LoadProject(projectPath);
				if (project == null || !project.Valid) continue;
				ExtractProject(project);
			}
		}
		public void LoadProjectsParallel(List<string> projectPaths)
		{
			List<Project> projects = new List<Project>();
			List<List<Project>> projectChunks = new List<List<Project>>();
			foreach(var projectPath in projectPaths)
			{
				var project = LoadProjectHeader(projectPath);
				if (project == null) continue;
				projects.Add(project);
			}
			List<Project> buffer = new List<Project>();
			for(int i = 0; i < projects.Count; i++)
			{
				buffer.Add(projects[i]);
				if ((i % 10 == 0 && i > 0) || i == projects.Count-1)
				{
					var chunk = new List<Project>();
					foreach(var project in buffer) {  chunk.Add(project); }
					projectChunks.Add(chunk);
					buffer.Clear();
				} 
			}
			foreach(var chunk in projectChunks)
			{
				Parallel.ForEach(chunk, project =>
				{
					project.Load(packFileCache);
				});
			}
			//Partitioner<Project> partitioner = Partitioner.Create(projects);

			//Parallel.ForEach(partitioner, (project, loopstate) =>
			//{
			//	project.Load(packFileCache);
			//});

			foreach (var project in projects)
			{
				ExtractProject(project);
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
		public Project? LoadProject(string projectFilePath)
		{
			if (String.IsNullOrWhiteSpace(projectFilePath)) return null;

			lock (projectMap)
			{

				var project = Project.Load(new FileInfo(Path.Join(templateFolder.FullName, projectFilePath)), packFileCache);

				projectMap.Add(project.Identifier, project);

				//lock (project) ExtractProject(project);
				return project;
			}

		}
		public Project? LoadProjectHeader(string projectFilePath)
		{
			if (String.IsNullOrEmpty(projectFilePath)) return null;
			lock (projectMap)
			{
				var project = new Project(packFileCache.LoadPackFile(new FileInfo(Path.Join(templateFolder.FullName, projectFilePath))));

				projectMap.Add(project.Identifier, project);

				return project;
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
			lock(folderProjectMap)
			{
				if (!folderProjectMap.ContainsKey(project.ProjectDirectory!.Name))
				{
					folderProjectMap.Add(project.ProjectDirectory!.Name, project);
				}
			}
		}
		private bool TryLookupNestedPackFile(string name, out PackFile? packFile)
		{
			packFile = null;
			string[] sections = name.Split('~');
			Project project; 
			if (!projectMap.TryGetValue(sections[0], out project!))
			{
				return false; 
			}
			return project.TryLookupPackFile(name, out packFile);
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

		public bool TryLookupPackFile(string name, out PackFile? packFile)
		{
			name = name.ToLower();
			packFile = null;
			if (name.Contains('~'))
			{
				return TryLookupNestedPackFile(name, out packFile);
			}
			Project project; 
			if (!fileProjectMap.TryGetValue(name, out project!)) { return false; }

			return project.TryLookupPackFile(name, out packFile);

		}
		public PackFile LookupPackFile(string name)
		{
			name = name.ToLower();
			return name.Contains('~') ? LookupNestedPackFile(name) : fileProjectMap[name].LookupPackFile(name);	
		}

		public bool ContainsPackFile(string name)
		{
			return name.Contains('~') ? ContainsNestedPackFile(name) : fileProjectMap.ContainsKey(name) && fileProjectMap[name].ContainsPackFile(name);
		}
		public bool TryLookupProjectFolder(string name, out Project? project)
		{
			return folderProjectMap.TryGetValue(name, out project);
		}
		public bool TryActivatePackFile(string name, Project project, out PackFile? packFile)
		{
			packFile = null;
			if (!project.TryLookupPackFile(name, out packFile))
			{
				return false;
			}
			lock (ActivePackFiles)
				lock (packFile)
				{
					if (ActivePackFiles.Contains(packFile)) { return true; }
					ActivePackFiles.Add(packFile);
					packFile.Activate();
				}
			return true;
		}
		public bool TryActivatePackFile(string name, out PackFile? packFile)
		{
			packFile = null; 
			if (!TryLookupPackFile(name, out packFile!))
			{
				return false; 
			}
			lock (ActivePackFiles)
				lock (packFile)
				{
					if (ActivePackFiles.Contains(packFile)) { return true; }
					ActivePackFiles.Add(packFile);
					packFile.Activate();
				}
			return true;
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
		public bool ActivatePackFile(PackFile packFile)
		{
			lock (ActivePackFiles)
				lock (packFile)
				{
					if (ActivePackFiles.Contains(packFile)) return true;
					ActivePackFiles.Add(packFile);
					packFile.Activate();
				}
			return false;
		}
		

		public bool ApplyPatches()
		{
			packFileCache.DeletePackFileOutput();

			

			Parallel.ForEach(ActivePackFiles, packFile =>
			{
				packFile.ApplyChanges();
				//packFile.Map.Save(Path.Join(Directory.GetCurrentDirectory(), packFile.InputHandle.Name));

			});
			//foreach (PackFile packFile in ActivePackFiles)
			//{
			//	packFile.ApplyChanges();
			//	packFile.Map.Save(Path.Join(Directory.GetCurrentDirectory(), packFile.InputHandle.Name));
			//	packFile.Export();
			//}
			return true;
		}

		public async Task<bool> ApplyPatchesParallel()
		{
			Task deleteOutputTask = Task.Run(packFileCache.DeletePackFileOutput);
			try
			{
				Parallel.ForEach(projectMap.Values, project => { fnisParser.ScanProjectAnimlist(project); });
			} 
			catch (Exception ex)
			{
				Logger.Error($"FNIS Parser > Scan > Failed > {ex.Message}");
			}
			

			Parallel.ForEach(ActivePackFiles, packFile =>
			{
				packFile.ApplyChanges();
			});

			await deleteOutputTask;


			return true;

		}
		public void SaveCache()
		{
			packFileCache.SavePackFileOutput(ActivePackFiles);
		}
	}
}
