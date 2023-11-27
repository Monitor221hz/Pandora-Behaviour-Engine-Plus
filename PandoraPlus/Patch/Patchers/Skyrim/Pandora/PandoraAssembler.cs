using Pandora.Core;
using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.AnimData;
using Pandora.Patch.Patchers.Skyrim.AnimSetData;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Pandora.Patch.Patchers.Skyrim.Pandora
{
	using ChangeType = IPackFileChange.ChangeType;
	public class PandoraAssembler
	{
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		private ProjectManager projectManager { get; set; }	
		private AnimDataManager animDataManager { get; set; }	
		private AnimSetDataManager animSetDataManager { get; set; }

		private DirectoryInfo engineFolder = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Pandora_Engine");

		private DirectoryInfo templateFolder = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Pandora_Engine\\Skyrim\\Template");

		private DirectoryInfo outputFolder = new DirectoryInfo($"{Directory.GetCurrentDirectory()}\\meshes");


		public PandoraAssembler()
		{
			projectManager = new ProjectManager(templateFolder, outputFolder);
			animSetDataManager = new AnimSetDataManager(templateFolder, outputFolder);
			animDataManager = new AnimDataManager(templateFolder, outputFolder);
		}

		public PandoraAssembler(ProjectManager projManager, AnimSetDataManager animSDManager, AnimDataManager animDManager)
		{
			this.projectManager = projManager;
			this.animSetDataManager = animSDManager;
			this.animDataManager = animDManager;
		}
		private IPackFileChange TagEdit(ChangeType changeType, string path, XElement element)
		{
			switch (changeType)
			{
				case ChangeType.Remove:
					return new RemoveElementChange(path);
				case ChangeType.Insert:
					return new InsertElementChange(path, element);
				case ChangeType.Replace:
					return new ReplaceElementChange(path, element);
				case ChangeType.Append:
					return new AppendElementChange(path, element);

			}

			return new RemoveElementChange(path);
		}

		private IPackFileChange TagEdit(ChangeType changeType, string path, string value)
		{
			switch (changeType)
			{
				//case ChangeType.Remove:
				//	return new RemoveTextChange(path, value);
				//case ChangeType.Insert:
				//	return new InsertTextChange(path, value);
				//case ChangeType.Replace:
				//	return new ReplaceElementChange(path, value);
				//case ChangeType.Append:
				//	return new AppendElementChange(path, value);

			}

			return new RemoveElementChange(path);
		}
		private bool AssembleUnknownEdits(DirectoryInfo folder, out List<XElement> elements, out List<string> strings, ChangeType changeType)
		{
			var textFiles = folder.GetFiles("*.txt");
			var xmlFiles = folder.GetFiles("*.xml");
			elements = new List<XElement>();
			strings = new List<string>(); 
			foreach ( var file in textFiles )
			{
				using (var readStream = file.OpenRead())
				{
					using (var reader = new StreamReader(readStream))
					{
						string? expectedLine; 
						while ((expectedLine = reader.ReadLine()) != null)
						{
							if (String.IsNullOrWhiteSpace(expectedLine)) continue; 
							strings.Add(expectedLine);
						}
					}
				}
			}
			foreach( var file in xmlFiles )
			{
				try
				{
					elements.Add(XElement.Load(file.FullName));
				}
				catch (Exception e) 
				{
					Logger.Error($"Pandora Assembler > File {file.FullName} > Load > FAILED > {e.Message}");
				}
			}
			return elements.Count > 0 || strings.Count > 0;
		}

		private void TagUnknownEdits(List<XElement> elements, List<string> strings, ChangeType changeType)
		{
			switch(changeType)
			{

			}
		}
		private void ForwardReplaceEdits(string path, PackFileChangeSet changeSet, List<XElement> elements, List<string> strings)
		{
			foreach(var element in elements )
			{
				changeSet.AddChange(new ReplaceElementChange(path,element));
			}
		}
		private void IdentifyUnknownEdits(string name, List<XElement> elements, List<string> values)
		{
			switch(name)
			{
				case "replace":
					TagUnknownEdits(elements, values, ChangeType.Replace);
					break;
				case "remove":
					TagUnknownEdits(elements, values, ChangeType.Remove);
					break;
				case "insert":
					TagUnknownEdits(elements, values, ChangeType.Insert);
					break;
				case "append":
					TagUnknownEdits(elements, values, ChangeType.Append);
					break;
				default:
					break;
			}
		}
		private bool AssemblePackFilePatch(DirectoryInfo folder, IModInfo modInfo)
		{
			if (!projectManager.ContainsPackFile(folder.Name)) return false;
			var changeSet = new PackFileChangeSet(modInfo);
			var modName = modInfo.Name;
			var targetPackFile = projectManager.ActivatePackFile(folder.Name);


			var subFolders = modInfo.Folder.GetDirectories();
			if (subFolders.Length == 0) return false;
			List<string> values;
			List<XElement> elements; 
			foreach ( var subFolder in subFolders )
			{
				if (!AssembleUnknownEdits(subFolder, out elements, out values, ChangeType.Remove)) continue; 

				switch (subFolder.Name.ToLower())
				{
					case "replace":
						TagUnknownEdits(elements, values, ChangeType.Replace);
						break;
					case "delete":
						TagUnknownEdits(elements, values, ChangeType.Remove);
						break;
					case "insert":
						break;
					case "append":
						break;
					default:
						break;
				}
			}



			return true;
		}

		public void AssembleAnimDataPatch(DirectoryInfo folder)
		{
			var files = folder.GetFiles();
			foreach (var file in files)
			{
				Project? targetProject;
				if (!file.Exists || !projectManager.TryGetProject(Path.GetFileNameWithoutExtension(file.Name.ToLower()), out targetProject)) continue;

				using (var readStream = file.OpenRead())
				{
					using (var reader = new StreamReader(readStream))
					{
						string? expectedLine;
						while ((expectedLine = reader.ReadLine()) != null)
						{
							if (String.IsNullOrWhiteSpace(expectedLine)) continue;
							targetProject!.AnimData?.AddDummyClipData(expectedLine);
						}
					}
				}
			}
		}
		public void AssembleAnimSetDataPatch(DirectoryInfo directoryInfo) //not exactly Nemesis format but this format is just simpler
		{
			ProjectAnimSetData? targetAnimSetData;

			foreach (DirectoryInfo subDirInfo in directoryInfo.GetDirectories())
			{
				if (!animSetDataManager.AnimSetDataMap.TryGetValue(subDirInfo.Name, out targetAnimSetData)) return;
				var patchFiles = subDirInfo.GetFiles();

				foreach (var patchFile in patchFiles)
				{
					AnimSet? targetAnimSet;
					if (!targetAnimSetData.AnimSetsByName.TryGetValue(patchFile.Name, out targetAnimSet)) continue;

					using (var readStream = patchFile.OpenRead())
					{

						using (var reader = new StreamReader(readStream))
						{

							string? expectedPath;
							while ((expectedPath = reader.ReadLine()) != null)
							{
								if (string.IsNullOrWhiteSpace(expectedPath)) continue;

								string animationName = Path.GetFileNameWithoutExtension(expectedPath);
								string folder = Path.GetDirectoryName(expectedPath)!;
								var animInfo = SetCachedAnimInfo.Encode(folder, animationName);
								targetAnimSet.AddAnimInfo(animInfo);
							}

						}
					}
				}
			}




		}
	}
}
