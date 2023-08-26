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

		private Dictionary<string, FileInfo> filesByName { get; set; } = new Dictionary<string, FileInfo>();  

		public string Identifier { get; private set; } = string.Empty;

		public bool Valid { get; private set; }

		private FileInfo projectFile {  get; set; }	
		private FileInfo characterFile { get; set; } 
		private FileInfo skeletonFile { get; set; }
		private FileInfo behaviorFile { get; set; } 

		public Project()
		{
#if DEBUG
			throw new InvalidDataException("Could not initialize project because of invalid data"); 
#endif
			Valid = false;
			projectFile = new FileInfo("");
			characterFile = projectFile; 
			skeletonFile = characterFile; 
			behaviorFile = skeletonFile;
		}
		public Project(FileInfo projectfile, FileInfo characterfile, FileInfo skeletonfile, FileInfo behaviorfile)
		{
			projectFile = projectfile;
			characterFile = characterfile;
			skeletonFile = skeletonfile;
			behaviorFile = behaviorfile;

			Identifier = Path.GetFileNameWithoutExtension(projectFile.Name);
			Valid = true;
		}
		
		public FileInfo LookupFileHandle(string name) => filesByName[name];

		public bool ContainsFileHandle(string name) => filesByName.ContainsKey(name);

		public List<string> MapFiles()
		{
			DirectoryInfo? behaviorFolder = behaviorFile.Directory;
			if (behaviorFolder == null) return new List<string>();

			var behaviorFiles = behaviorFolder.GetFiles();

			foreach ( var behaviorFile in behaviorFiles)
			{
				filesByName.Add(Path.GetFileNameWithoutExtension(behaviorFile.Name), behaviorFile); 
			}
			return filesByName.Keys.ToList();
		}
		public static Project LoadFrom(FileInfo projectFile)
		{
			if (!projectFile.Exists) return new Project(); 

			FileInfo characterFile = GetCharacterFile(projectFile); 
			if (!characterFile.Exists) return new Project();

			var rigBehaviorPair = GetSkeletonAndBehaviorFile(projectFile, characterFile);

			FileInfo skeletonFile = rigBehaviorPair.skeleton; 
			if (!skeletonFile.Exists) return new Project();

			FileInfo behaviorFile = rigBehaviorPair.behavior; 
			if (!behaviorFile.Exists) return new Project();

			return new Project(projectFile, characterFile, skeletonFile, behaviorFile);
		}

		public static Project Load(string projectFilePath) => LoadFrom(new FileInfo(projectFilePath));


		private static FileInfo GetCharacterFile(FileInfo projectFile)
		{


			XMap projectMap = XMap.Load(projectFile.FullName);
			XElement projectData = projectMap.NavigateTo("hkbProjectStringData", projectMap.NavigateTo("__data__"), (x) => XMap.TryGetAttributeName("class", x));
			projectMap.MapSlice(projectData, true);

			string characterFilePath = projectMap.Lookup("characterFilenames").Value;
			FileInfo characterFile = new FileInfo(Path.Combine(projectFile.DirectoryName!, characterFilePath));
			//{C:\Users\Monitor\source\repos\Pandora-Plus-Behavior-Engine\PandoraPlus\bin\Debug\net7.0-windows\Pandora_Engine\Skyrim\Template\actors\character\Characters\Character Assets\skeleton.HKX}
			return characterFile; 
		}

		private static (FileInfo skeleton, FileInfo behavior) GetSkeletonAndBehaviorFile(FileInfo projectFile, FileInfo characterFile)
		{
			XMap characterMap =  XMap.Load(characterFile.FullName);

			XElement characterData = characterMap.NavigateTo("hkbCharacterStringData", characterMap.NavigateTo("__data__"), (x) => XMap.TryGetAttributeName("class", x));

			string skeletonFilePath = characterMap.NavigateTo("rigName", characterData).Value;

			string behaviorFilePath = characterMap.NavigateTo("behaviorFilename", characterData).Value;

			return (new FileInfo(Path.Combine(projectFile.DirectoryName!, skeletonFilePath)), new FileInfo(Path.Combine(projectFile.DirectoryName!, behaviorFilePath)));

		}

	}
}
