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
		public ProjectManager ProjectManager { get; private set; }	
		public AnimDataManager AnimDataManager { get; private set; }	
		public AnimSetDataManager AnimSetDataManager { get; private set; }

		private DirectoryInfo engineFolder = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Pandora_Engine");

		private DirectoryInfo templateFolder = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Pandora_Engine\\Skyrim\\Template");

		private DirectoryInfo outputFolder = new DirectoryInfo($"{Directory.GetCurrentDirectory()}\\meshes");

		private Dictionary<string, FileInfo> cachedFiles = new Dictionary<string, FileInfo>();

		private static readonly string stateMachineChildrenFormatPath = "{0}/states";

		private static readonly Dictionary<string, ChangeType> changeTypeNameMap =  Enum.GetValues(typeof(ChangeType)).Cast<ChangeType>().ToDictionary(c => c.ToString(), v => v, StringComparer.OrdinalIgnoreCase);

		public PandoraAssembler()
		{
			ProjectManager = new ProjectManager(templateFolder, outputFolder);
			AnimSetDataManager = new AnimSetDataManager(templateFolder, outputFolder);
			AnimDataManager = new AnimDataManager(templateFolder, outputFolder);
		}
		public PandoraAssembler(NemesisAssembler nemesisAssembler)
		{
			ProjectManager = nemesisAssembler.ProjectManager;
			AnimSetDataManager = nemesisAssembler.AnimSetDataManager;
			AnimDataManager = nemesisAssembler.AnimDataManager;
		}
		public PandoraAssembler(ProjectManager projManager, AnimSetDataManager animSDManager, AnimDataManager animDManager)
		{
			this.ProjectManager = projManager;
			this.AnimSetDataManager = animSDManager;
			this.AnimDataManager = animDManager;
		}
		public void AssembleEdit(ChangeType changeType, XElement element, PackFileChangeSet changeSet)
		{
			XAttribute? pathAttribute = element.Attribute("path");
			if (pathAttribute == null) { return; }

			bool isPathEmpty = string.IsNullOrWhiteSpace(pathAttribute.Value);

			XAttribute? textAttribute = element.Attribute("text");
			XAttribute? preTextAttribute = element.Attribute("preText");

			switch (changeType)
			{
				case ChangeType.Remove:
					if (textAttribute == null)
					{
						changeSet.AddChange(new RemoveElementChange(pathAttribute.Value));
						break;
					}
					//assume text
					if (String.IsNullOrWhiteSpace(element.Value) || String.IsNullOrWhiteSpace(textAttribute.Value)) { break; }

					if (preTextAttribute == null)
					{
						changeSet.AddChange(new RemoveTextChange(pathAttribute.Value, textAttribute.Value));
						break;
					}
					changeSet.AddChange(new ReplaceTextChange(pathAttribute.Value, preTextAttribute.Value, textAttribute.Value, string.Empty));

					break;

				case ChangeType.Insert:
					if (element.IsEmpty) { break; }
					if (element.HasElements)
					{
						if (!isPathEmpty)
						{
							foreach (var childElement in element.Elements()) { changeSet.AddChange(new InsertElementChange(pathAttribute.Value, childElement)); }
							break;
						}

						foreach (var childElement in element.Elements()) { changeSet.AddChange(new PushElementChange(PackFile.ROOT_CONTAINER_NAME, element)); }
						break;
					}
					if (textAttribute == null || isPathEmpty) { break; }

					changeSet.AddChange(new InsertTextChange(pathAttribute.Value, textAttribute.Value, element.Value));

					break;
				case ChangeType.Append:
					if (element.IsEmpty) { break; }
					if (element.HasElements)
					{
						if (!isPathEmpty)
						{
							foreach (var childElement in element.Elements()) { changeSet.AddChange(new AppendElementChange(pathAttribute.Value, childElement));  }
							break;
						}

						foreach (var childElement in element.Elements()) { changeSet.AddChange(new PushElementChange(PackFile.ROOT_CONTAINER_NAME, element)); }
						break;
					}

					if (isPathEmpty) { break; }
					changeSet.AddChange(new AppendTextChange(pathAttribute.Value, element.Value));

					break;

				case ChangeType.Replace:
					if (element.IsEmpty || isPathEmpty) { break; }
					if (textAttribute == null && element.HasElements)
					{
						foreach(var childElement in element.Elements()) { changeSet.AddChange(new ReplaceElementChange(pathAttribute.Value, new XElement(childElement))); } 
						break;
					}
					if (textAttribute == null) { break; }
					if (preTextAttribute == null)
					{
						changeSet.AddChange(new ReplaceTextChange(pathAttribute.Value, string.Empty, textAttribute.Value, element.Value));
						break;
					}
					changeSet.AddChange(new ReplaceTextChange(pathAttribute.Value, preTextAttribute.Value, textAttribute.Value, element.Value));
					break;

				default:
					break;


			}
		}

		public void AssembleTypedEdits(ChangeType changeType, XElement container, PackFileChangeSet changeSet)
		{
			foreach (var element in container.Elements())
			{
				AssembleEdit(changeType, element, changeSet);
			}
		}

		public void AssembleEdits(XElement container, PackFileChangeSet changeSet)
		{
			if (!container.HasElements) { return; }	
			foreach (var element in container.Elements())
			{

				if (changeTypeNameMap.TryGetValue(element.Name.ToString(), out ChangeType changeType))
				{
					if (element.HasAttributes) 
					{
						AssembleEdit(changeType, element, changeSet);
						continue;
					}
					AssembleTypedEdits(changeType, element, changeSet);
					continue;
				}
				AssembleEdits(element, changeSet);
				
			}
		}
		public bool AssemblePackFilePatch(FileInfo file, IModInfo modInfo)
		{

			var name = Path.GetFileNameWithoutExtension(file.Name);
			PackFile targetPackFile; 
			if (!ProjectManager.TryActivatePackFile(name, out targetPackFile!)) { return false; }

			var changeSet = new PackFileChangeSet(modInfo);

			XElement container;
			using (FileStream stream = file.OpenRead())
			{
				container = XElement.Load(stream);
			}
			var editContainer = container;

			if (editContainer == null) { return false;  }

			AssembleEdits(editContainer, changeSet);

			targetPackFile.Dispatcher.AddChangeSet(changeSet);
			return true;
		}
		public void AssemblePatch(IModInfo modInfo)
		{
			var patchFolder = new DirectoryInfo(Path.Join(modInfo.Folder.FullName, "patches"));
			foreach( var file in patchFolder.GetFiles("*.xml"))
			{
				AssemblePackFilePatch(file, modInfo);
			}
		}
		public void AssembleAnimDataPatch(DirectoryInfo folder)
		{
			var files = folder.GetFiles();
			foreach (var file in files)
			{
				Project? targetProject;
				if (!file.Exists || !ProjectManager.TryGetProject(Path.GetFileNameWithoutExtension(file.Name.ToLower()), out targetProject)) continue;

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
				if (!AnimSetDataManager.AnimSetDataMap.TryGetValue(subDirInfo.Name, out targetAnimSetData)) continue;
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
			foreach (FileInfo patchFile in directoryInfo.GetFiles("*.txt"))
			{
				if (!AnimSetDataManager.AnimSetDataMap.TryGetValue(Path.GetFileNameWithoutExtension(patchFile.Name), out targetAnimSetData)) continue;
				List<SetCachedAnimInfo> animInfos = new();
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
							animInfos.Add(animInfo);
						}
					}
				}
				foreach (var animSet in targetAnimSetData.AnimSets)
				{
					foreach (var animInfo in animInfos)
					{
						animSet.AddAnimInfo(animInfo);
					}
				}
				break;
			}



		}
	}
}
