using HKX2;
using Pandora.Core;
using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.AnimData;
using Pandora.Patch.Patchers.Skyrim.AnimSetData;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using Pandora.Patch.Patchers.Skyrim.Nemesis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

		private Dictionary<string, FileInfo> cachedFiles = new Dictionary<string, FileInfo>();

		private static readonly string stateMachineChildrenFormatPath = "{0}/states";

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

		private bool TryLoadPackedFileGraph(FileInfo file, out PackFileGraph? packFile)
		{
			FileInfo? existingFile;
			bool cached = cachedFiles.TryGetValue(file.Name, out existingFile);
			if (cached) { file = existingFile!; }

			var unpackedFile = new FileInfo(Path.ChangeExtension(file.FullName, ".xml"));
			packFile = null;
			
			try
			{
				packFile = unpackedFile.Exists ? new PackFileGraph(unpackedFile) : new PackFileGraph(PackFile.GetUnpackedHandle(file));
				packFile.Activate();

				if (!cached) { cachedFiles.Add(file.Name, file); }

			}
			catch (Exception ex)
			{
				//add log message later
				return false;	
			}
			return true;
		}
		private bool TryLoadPackFileGraph(FileInfo file, out PackFileGraph? packFile)
		{
			packFile = null; 
			try
			{
				packFile = new PackFileGraph(file);
				packFile.Activate();
			}
			catch 
			(Exception ex)
			{ 
				return false; 
			}
			return true;
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
		private void InjectEventsAndVariables(PackFileGraph sourcePackFile, PackFileGraph destPackFile, PackFileChangeSet destChangeSet)
		{
			var eventNameElements = sourcePackFile.EventNames;
			var eventFlagElements = sourcePackFile.EventFlags;

			var variableNameElements = sourcePackFile.VariableNames;
			var variableValueElements = sourcePackFile.VariableValues;
			var variableTypeElements = sourcePackFile.VariableTypes;

			for (int i = 0; i < eventNameElements.Count; i++)
			{
				destChangeSet.AddChange(new AppendElementChange(destPackFile.EventNamesPath, eventNameElements[i]));
				destChangeSet.AddChange(new AppendElementChange(destPackFile.EventFlagsPath, eventFlagElements[i]));
			}

			for (int i = 0; i < variableNameElements.Count; i++)
			{
				destChangeSet.AddChange(new AppendElementChange(destPackFile.VariableNamesPath, variableNameElements[i]));
				destChangeSet.AddChange(new AppendElementChange(destPackFile.VariableValuesPath, variableValueElements[i]));
				destChangeSet.AddChange(new AppendElementChange(destPackFile.VariableTypesPath, variableTypeElements[i]));
			}
		}

		private void InjectGraphAnimations(PackFileGraph sourcePackFile, PackFileCharacter destPackFile, PackFileChangeSet changeSet)
		{
			var animationNames = sourcePackFile.GetAnimationFilePaths();
			//projectManager.ActivatePackFile((PackFile)destPackFile);
			foreach(var animationName in animationNames) { changeSet.AddChange(new AppendElementChange(destPackFile.AnimationNamesPath, new XElement("hkcstring", animationName))); }
		}
		private void InjectGraphReference(PackFileGraph sourcePackFile, PackFileGraph destPackFile, PackFileChangeSet changeSet, string stateFolderName)
		{
			InjectEventsAndVariables(sourcePackFile, destPackFile, changeSet);
			string nameWithoutExtension = Path.GetFileNameWithoutExtension(sourcePackFile.OutputHandle.Name);
			string refName = nameWithoutExtension.Replace(' ', '_');
			var stateInfoPath = string.Format(stateMachineChildrenFormatPath, stateFolderName);
			var graphPath = $"{destPackFile.OutputHandle.Directory?.Name}\\{nameWithoutExtension}.hkx";


			PatchNodeCreator nodeMaker = new PatchNodeCreator(changeSet.Origin.Code);

			string behaviorRefName;
			var behaviorRef = nodeMaker.CreateBehaviorReferenceGenerator(refName, graphPath, out behaviorRefName);
			XElement behaviorRefElement = nodeMaker.TranslateToLinq<hkbBehaviorReferenceGenerator>(behaviorRef, behaviorRefName);

			string stateInfoName;
			var stateInfo = nodeMaker.CreateSimpleStateInfo(behaviorRef, out stateInfoName);
			XElement stateInfoElement = nodeMaker.TranslateToLinq<hkbStateMachineStateInfo>(stateInfo, stateInfoName);

			changeSet.AddChange(new AppendElementChange(PackFile.ROOT_CONTAINER_NAME, behaviorRefElement));
			changeSet.AddChange(new AppendElementChange(PackFile.ROOT_CONTAINER_NAME, stateInfoElement));
			changeSet.AddChange(new AppendTextChange(stateInfoPath, stateInfoName));
		}
		public void AssembleGraphInjection(DirectoryInfo injectFolder, PackFile destPackFile, PackFileChangeSet changeSet)
		{
			if (destPackFile is not PackFileGraph && destPackFile is not PackFileCharacter) { return; }


			if (destPackFile is PackFileCharacter)
			{
				foreach (var file in injectFolder.GetFiles("*.xml"))
				{
					PackFileGraph? sourcePackFile; 

					if (!TryLoadPackFileGraph(file, out sourcePackFile)) { continue; }

					InjectGraphAnimations(sourcePackFile!, (PackFileCharacter)destPackFile, changeSet);
				}
				return; 
			}

			var stateFolders = injectFolder.GetDirectories();

			foreach (var stateFolder in stateFolders)
			{

				destPackFile.MapNode(stateFolder.Name);

				foreach(var file in stateFolder.GetFiles("*.xml"))
				{
					PackFileGraph? sourcePackFile;

					if (!TryLoadPackFileGraph(file, out sourcePackFile)) { continue; }

					InjectGraphReference(sourcePackFile!, (PackFileGraph)destPackFile, changeSet, stateFolder.Name);
				}
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
					case "inject": //special case for injecting behavior files
						AssembleGraphInjection(subFolder, targetPackFile, changeSet);
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
