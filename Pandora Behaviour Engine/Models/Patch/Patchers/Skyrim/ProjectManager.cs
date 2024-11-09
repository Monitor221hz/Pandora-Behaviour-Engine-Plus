using NLog;
using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Skyrim64;
using Pandora.Patch.Patchers.Skyrim.FNIS;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Core.Patchers.Skyrim
{
	public class ProjectManager : IProjectManager
	{
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		private Dictionary<string, Project> projectMap { get; set; } = new Dictionary<string, Project>();
		private Dictionary<string, Project> fileProjectMap { get; set; } = new Dictionary<string, Project>();

		private Dictionary<string, Project> folderProjectMap { get; set; } = new Dictionary<string, Project>();

		private Dictionary<string, List<Project>> linkedProjectMap { get; set; } = new Dictionary<string, List<Project>>();

		private readonly DirectoryInfo templateFolder;

		private DirectoryInfo baseOutputDirectory;


		private PackFileCache packFileCache { get; set; } = new();


		private FNISParser fnisParser;

		private bool CompleteExportSuccess = true;
		public HashSet<PackFile> ActivePackFiles { get; private set; } = new HashSet<PackFile>();

		public ProjectManager(DirectoryInfo templateFolder, DirectoryInfo outputDirectory)
		{
			this.templateFolder = templateFolder;
			this.baseOutputDirectory = outputDirectory;
			fnisParser = new FNISParser(this, baseOutputDirectory);
		}
		public void GetExportInfo(StringBuilder builder)
		{
			if (CompleteExportSuccess) { return; }
			builder.AppendLine();

			foreach (var failedPackFile in ActivePackFiles.Where(pf => !pf.ExportSuccess))
			{
				builder.AppendLine($"FATAL ERROR: Could not export {failedPackFile.UniqueName}. Check Engine.log for more information.");
			}
		}
		public void GetAnimationInfo(StringBuilder builder)
		{
			var projects = projectMap.Values;
			uint totalAnimationCount = 0;

			builder.AppendLine();
			foreach (var project in projects)
			{
				var animCount = project.CharacterPackFile.NewAnimationCount;
				if (animCount == 0) { continue; }
				totalAnimationCount += animCount;
				builder.AppendLine($"{animCount} animations added to {project.Identifier}.");
			}
			builder.AppendLine();
			builder.AppendLine($"{totalAnimationCount} total animations added.");
		}

		public void GetFNISInfo(StringBuilder builder)
		{
			uint fnisModCount = 0;
			builder.AppendLine();
			foreach (IModInfo modInfo in fnisParser.ModInfos)
			{
				fnisModCount++;
				builder.AppendLine($"FNIS Mod {fnisModCount} : {modInfo.Code}");
				Logger.Info($"FNIS Mod {fnisModCount} : {modInfo.Code}");
			}
		}

		public bool TryGetProject(string name, [NotNullWhen(true)] out Project? project) => projectMap.TryGetValue(name, out project);

		public bool TryGetProjectEx(string name, [NotNullWhen(true)] out IProject? project)
		{
			project = TryGetProject(name, out var exProject) ? exProject as IProject : null;
			return project != null; 
		}
		public bool ProjectExists(string name) => projectMap.ContainsKey(name);

		internal void LoadTrackedProjects()
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
			Dictionary<string, Project> directoryProjectPaths = new();
			foreach (var projectPath in projectPaths)
			{
				var project = LoadProject(projectPath);
				if (project == null || !project.Valid) continue;
				ExtractProject(project);
				if (project.ProjectDirectory == null) continue;
				var projectFolderPath = project.ProjectDirectory.FullName;
				if (directoryProjectPaths.TryGetValue(projectFolderPath, out var existingProject))
				{
					existingProject.Sibling = project;
					project.Sibling = existingProject;
				}
				else
				{
					directoryProjectPaths.Add(projectFolderPath, project);
				}
			}
		}
		internal void LoadProjectsParallel(List<string> projectPaths)
		{
			List<Project> projects = new List<Project>();
			List<List<Project>> projectChunks = new List<List<Project>>();
			foreach (var projectPath in projectPaths)
			{
				var project = LoadProjectHeader(projectPath);
				if (project == null) continue;
				projects.Add(project);
			}
			List<Project> buffer = new List<Project>();
			for (int i = 0; i < projects.Count; i++)
			{
				buffer.Add(projects[i]);
				if ((i % 10 == 0 && i > 0) || i == projects.Count - 1)
				{
					var chunk = new List<Project>();
					foreach (var project in buffer) { chunk.Add(project); }
					projectChunks.Add(chunk);
					buffer.Clear();
				}
			}
			foreach (var chunk in projectChunks)
			{
				Parallel.ForEach(chunk, project =>
				{
					project.Load(packFileCache);
				});
			}

			foreach (var project in projects)
			{
				ExtractProject(project);
			}
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
		public IProject? LoadProjectEx(string projectFilePath) => LoadProject(projectFilePath) as IProject;
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

		public IProject? LoadProjectHeaderEx(string projectFilePath) => LoadProjectHeader(projectFilePath) as IProject;
		public bool TryLoadOutputPackFile<T>(IPackFile packFile,[NotNullWhen(true)] out T? outPackFile) where T : class, IPackFile
		{
			var fileInfo = BehaviourEngine.SkyrimGameDirectory != null ? packFile.GetRebasedOutput(BehaviourEngine.SkyrimGameDirectory) : packFile.OutputHandle; 
			if (!fileInfo.Exists)
			{
				outPackFile = default;
				return false;
			}
			outPackFile = (packFileCache.LoadPackFile(fileInfo) as T)!;
			return true; 
		}
		public bool TryLoadOutputPackFile<T>(IPackFile packFile, string extension, [NotNullWhen(true)] out T? outPackFile) where T : class, IPackFile
		{
			var fileInfo = new FileInfo(Path.ChangeExtension((BehaviourEngine.SkyrimGameDirectory != null ? packFile.GetRebasedOutput(BehaviourEngine.SkyrimGameDirectory) : packFile.OutputHandle).FullName, extension));
			if (!fileInfo.Exists)
			{
				outPackFile = default;
				return false;
			}
			outPackFile = (packFileCache.LoadPackFile(fileInfo) as T)!;
			return true;
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
			lock (folderProjectMap)
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
		public bool TryLookupPackFile(string projectName, string packFileName, out PackFile? packFile)
		{
			packFile = null;
			Project project;
			if (!projectMap.TryGetValue(projectName, out project!))
			{
				return false;
			}
			return project.TryLookupPackFile(packFileName, out packFile);
		}
		public bool TryLookupPackFileEx(string name, string packFileName, out IPackFile? packFile)
		{
			packFile = TryLookupPackFile(name, packFileName, out var exPackFile) ? exPackFile as IPackFile : null;
			return packFile != null;
		}
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

		public bool TryLookupPackFileEx(string name, out IPackFile? packFile)
		{
			packFile = TryLookupPackFile(name, out var exPackFile) ? exPackFile as IPackFile : null;
			return packFile != null;
		}
		public bool TryLookupProjectFolder(string folderName, out Project? project)
		{
			return folderProjectMap.TryGetValue(folderName, out project);
		}
		public bool TryLookupProjectFolderEx(string folderName, out IProject? project)
		{
			project = TryLookupProjectFolder(folderName, out var exProject) ? exProject as IProject : null;
			return project != null;
		}
		internal bool TryActivatePackFilePriority(string name, Project project, out PackFile? packFile)
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
					packFile.Activate();
					packFile.PopPriorityXmlAsObjects();
					ActivePackFiles.Add(packFile);

				}
			return true;
		}
		internal bool TryActivatePackFilePriority(string name, out PackFile? packFile)
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
					packFile.Activate();
					packFile.PopPriorityXmlAsObjects();
					ActivePackFiles.Add(packFile);
				}
			return true;
		}
		public bool TryActivatePackFile(PackFile packFile)
		{
			lock (ActivePackFiles)
				lock (packFile)
				{
					if (ActivePackFiles.Contains(packFile)) return true;
					packFile.Activate();
					ActivePackFiles.Add(packFile);

				}
			return false;
		}

		public bool TryActivatePackFileEx(IPackFile packFile) => TryActivatePackFile((PackFile)packFile); 

		public bool ApplyPatches()
		{

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

		internal bool ApplyPatchesParallel()
		{


			Parallel.ForEach(ActivePackFiles, packFile =>
			{
				packFile.ApplyChanges();
			});
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
				packFile.PushXmlAsObjects();
			});

			return true;

		}


		internal void SetOutputPath(DirectoryInfo baseDirectory)
		{
			this.baseOutputDirectory = baseDirectory;
			fnisParser.SetOutputPath(baseDirectory);
		}
	}
}
