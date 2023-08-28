using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

		private PackFile projectFile {  get; set; }	
		private PackFile characterFile { get; set; } 
		private PackFile skeletonFile { get; set; }
		private PackFile behaviorFile { get; set; } 

		public Project()
		{
			Valid = false;
			projectFile = new PackFile("");
			characterFile = projectFile; 
			skeletonFile = characterFile; 
			behaviorFile = skeletonFile;
#if DEBUG
			throw new InvalidDataException("Could not initialize project because of invalid data");
#endif
		}
		public Project(PackFile projectfile, PackFile characterfile, PackFile skeletonfile, PackFile behaviorfile)
		{
			projectFile = projectfile;
			characterFile = characterfile;
			skeletonFile = skeletonfile;
			behaviorFile = behaviorfile;

			Identifier = Path.GetFileNameWithoutExtension(projectFile.Handle.Name);
			Valid = true;
		}
		
		public PackFile LookupPackFile(string name) => filesByName[name];

		public bool ContainsPackFile(string name) => filesByName.ContainsKey(name);

		public List<string> MapFiles()
		{
			DirectoryInfo? behaviorFolder = behaviorFile.Handle.Directory;
			if (behaviorFolder == null) return new List<string>();

			var behaviorFiles = behaviorFolder.GetFiles();

			foreach ( var behaviorFile in behaviorFiles)
			{
				filesByName.Add(Path.GetFileNameWithoutExtension(behaviorFile.Name), new PackFile(behaviorFile)); 
			}
			return filesByName.Keys.ToList();
		}
		public static Project Load(PackFile projectFile)
		{
			if (!projectFile.Handle.Exists) return new Project(); 

			PackFile characterFile = GetCharacterFile(projectFile); 
			if (!characterFile.Handle.Exists) return new Project();

			var rigBehaviorPair = GetSkeletonAndBehaviorFile(projectFile, characterFile);

			PackFile skeletonFile = rigBehaviorPair.skeleton; 
			if (!skeletonFile.Handle.Exists) return new Project();

			PackFile behaviorFile = rigBehaviorPair.behavior; 
			if (!behaviorFile.Handle.Exists) return new Project();

			return new Project(projectFile, characterFile, skeletonFile, behaviorFile);
		}

		public static Project Load(string projectFilePath) => Load(new PackFile(projectFilePath));


		private static PackFile GetCharacterFile(PackFile projectFile)
		{


			XMap projectMap = projectFile.Map;
			XElement projectData = projectFile.GetNodeByClass("hkbProjectStringData"); 
			projectMap.MapSlice(projectData, true);

			string characterFilePath = projectMap.Lookup("characterFilenames").Value;
			PackFile characterFile = new PackFile(Path.Combine(projectFile.Handle.DirectoryName!, characterFilePath));
			//{C:\Users\Monitor\source\repos\Pandora-Plus-Behavior-Engine\PandoraPlus\bin\Debug\net7.0-windows\Pandora_Engine\Skyrim\Template\actors\character\Characters\Character Assets\skeleton.HKX}
			return characterFile; 
		}

		private static (PackFile skeleton, PackFile behavior) GetSkeletonAndBehaviorFile(PackFile projectFile, PackFile characterFile)
		{
			XMap characterMap =  characterFile.Map;

			XElement characterStringDataNode = characterFile.GetNodeByClass("hkbCharacterStringData"); 

			string skeletonFilePath = characterMap.NavigateTo("rigName", characterStringDataNode).Value;

			string behaviorFilePath = characterMap.NavigateTo("behaviorFilename", characterStringDataNode).Value;

			return (new PackFile(Path.Combine(projectFile.Handle.DirectoryName!, skeletonFilePath)), new PackFile(Path.Combine(projectFile.Handle.DirectoryName!, behaviorFilePath)));

		}

	}
}
