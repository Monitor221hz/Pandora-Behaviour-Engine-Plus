using Pandora.Patch.Patchers.Skyrim.AnimData;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XmlCake.Linq;

namespace Pandora.Core.Patchers.Skyrim
{
	public class Project
	{

		private Dictionary<string, PackFile> filesByName { get; set; } = new Dictionary<string, PackFile>();  

		public string Identifier { get; private set; } = string.Empty;

		public bool Valid { get; private set; }

		public PackFile ProjectFile {  get; private set; }

		public DirectoryInfo? ProjectDirectory => ProjectFile?.InputHandle.Directory;

		public DirectoryInfo? OutputDirectory => ProjectFile?.OutputHandle.Directory;

		public DirectoryInfo? OutputBehaviorDirectory => BehaviorFile?.OutputHandle.Directory;

		public DirectoryInfo? OutputAnimationDirectory => new DirectoryInfo(Path.Join(OutputDirectory?.FullName, "animations"));

		public PackFileCharacter CharacterFile { get; private set; } 
		public PackFile SkeletonFile { get; private set; }
		public PackFileGraph BehaviorFile { get; private set; } 

		public ProjectAnimData? AnimData { get; set; }

		public Project()
		{
			Valid = false;
		}
		public Project(PackFile projectFile)
		{
			Valid = false;
			ProjectFile = projectFile;
			Identifier = Path.GetFileNameWithoutExtension(ProjectFile.InputHandle.Name);
		}
		public Project(PackFile projectfile, PackFileCharacter characterfile, PackFile skeletonfile, PackFileGraph behaviorfile)
		{
			ProjectFile = projectfile;
			CharacterFile = characterfile;
			SkeletonFile = skeletonfile;
			BehaviorFile = behaviorfile;

			Identifier = Path.GetFileNameWithoutExtension(ProjectFile.InputHandle.Name);
			Valid = true;
		}
		
		public PackFile LookupPackFile(string name) => filesByName[name];

		public bool TryLookupPackFile(string name, out PackFile? packFile) => filesByName.TryGetValue(name, out packFile);

		public bool ContainsPackFile(string name) => filesByName.ContainsKey(name);

		public List<string> MapFiles(PackFileCache cache)
		{
			DirectoryInfo? behaviorFolder = BehaviorFile.InputHandle.Directory;
			if (behaviorFolder == null) return new List<string>();

			var behaviorFiles = behaviorFolder.GetFiles("*.hkx");


			lock(filesByName)
			{
				foreach (var behaviorFile in behaviorFiles)
				{
					var packFile = cache.LoadPackFileGraph(behaviorFile, this);

					//packFile.DeleteExistingOutput();
					filesByName.Add(packFile.Name, packFile);
				}

				if (!filesByName.ContainsKey(SkeletonFile.Name)) filesByName.Add(SkeletonFile.Name, SkeletonFile);
				if (!filesByName.ContainsKey(CharacterFile.Name)) filesByName.Add(CharacterFile.Name, CharacterFile);

				filesByName.Add($"{Identifier}_skeleton", SkeletonFile);
				filesByName.Add($"{Identifier}_character", CharacterFile);
			


			//SkeletonFile.DeleteExistingOutput();
			//CharacterFile.DeleteExistingOutput();

			return filesByName.Keys.ToList();
			}
		}
		public static Project Create(PackFile projectFile, PackFileCache cache)
		{
			if (!projectFile.InputHandle.Exists) return new Project(); 

			PackFileCharacter characterFile = GetCharacterFile(projectFile, cache); 
			if (!characterFile.InputHandle.Exists) return new Project();

			var rigBehaviorPair = GetSkeletonAndBehaviorFile(projectFile, characterFile, cache);

			PackFile skeletonFile = rigBehaviorPair.skeleton; 
			if (!skeletonFile.InputHandle.Exists) return new Project();

			PackFileGraph behaviorFile = rigBehaviorPair.behavior; 
			if (!behaviorFile.InputHandle.Exists) return new Project();

			var project = new Project(projectFile, characterFile, skeletonFile, behaviorFile);

			projectFile.ParentProject = project;
			characterFile.ParentProject = project;
			skeletonFile.ParentProject = project;
			behaviorFile.ParentProject = project;

			return project;
		}
		public bool Load(PackFileCache cache)
		{
			if (!ProjectFile.InputHandle.Exists) return false;

			ProjectFile = ProjectFile;
			CharacterFile = GetCharacterFile(ProjectFile, cache);
			if (!CharacterFile.InputHandle.Exists) return false;

			var (skeleton, behavior) = GetSkeletonAndBehaviorFile(ProjectFile, CharacterFile, cache);

			SkeletonFile = skeleton;
			if (!SkeletonFile.InputHandle.Exists) return false;

			BehaviorFile = behavior;
			if (!BehaviorFile.InputHandle.Exists) return false;

			ProjectFile.ParentProject = this;
			CharacterFile.ParentProject = this;
			SkeletonFile.ParentProject = this;
			BehaviorFile.ParentProject = this;

			return true;
		}
		public static Project Load(FileInfo file, PackFileCache cache) => Create(cache.LoadPackFile(file), cache);

		//public static Project Load(string projectFilePath) => Load(new PackFile(projectFilePath));


		private static PackFileCharacter GetCharacterFile(PackFile projectFile, PackFileCache cache)
		{


			XMap projectMap = projectFile.Map;
			XElement projectData = projectFile.GetFirstNodeOfClass("hkbProjectStringData"); 
			projectMap.MapSlice(projectData, true);

			string characterFilePath = projectMap.Lookup("characterFilenames").Value.ToLower();

			return cache.LoadPackFileCharacter(new FileInfo(Path.Combine(projectFile.InputHandle.DirectoryName!, characterFilePath)));
		}

		private static (PackFile skeleton, PackFileGraph behavior) GetSkeletonAndBehaviorFile(PackFile projectFile, PackFileCharacter characterFile, PackFileCache cache)
		{
			XMap characterMap =  characterFile.Map;

			string skeletonFilePath = characterMap.Lookup(characterFile.RigNamePath).Value;

			string behaviorFilePath = characterMap.Lookup(characterFile.BehaviorFilenamePath).Value;

			return (cache.LoadPackFile(new FileInfo(Path.Combine(projectFile.InputHandle.DirectoryName!, skeletonFilePath))), cache.LoadPackFileGraph(new FileInfo(Path.Combine(projectFile.InputHandle.DirectoryName!, behaviorFilePath))));

		}

	}
}
