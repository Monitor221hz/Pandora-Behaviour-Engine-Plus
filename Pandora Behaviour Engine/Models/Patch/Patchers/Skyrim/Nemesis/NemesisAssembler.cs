using NLog;
using Pandora.API.Patch;
using Pandora.API.Patch.IOManagers;
using Pandora.Core;
using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.IOManagers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.AnimData;
using Pandora.Patch.Patchers.Skyrim.AnimSetData;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using Pandora.Patch.Patchers.Skyrim.Pandora;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using XmlCake.Linq;
using XmlCake.Linq.Expressions;

namespace Pandora.Patch.Patchers.Skyrim.Nemesis;

public class NemesisAssembler : IAssembler //animdata and animsetdata deviate from nemesis format
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger(); //to do: move logger into inheritable base class


	private IXExpression replacePattern = new XSkipWrapExpression(new XStep(XmlNodeType.Comment, "CLOSE"),new XStep(XmlNodeType.Comment, "OPEN"), new XStep(XmlNodeType.Comment, "ORIGINAL"), new XStep(XmlNodeType.Comment, "CLOSE"));
    private IXExpression insertPattern = new XSkipWrapExpression(new XStep(XmlNodeType.Comment, "ORIGINAL"), new XStep(XmlNodeType.Comment, "OPEN"), new XStep(XmlNodeType.Comment, "CLOSE")); 

    //private XPathLookup lookup = new XPathLookup();
    
    List<PackFile> packFiles = new List<PackFile>();

	private static readonly DirectoryInfo engineFolder = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Pandora_Engine");

	private static readonly DirectoryInfo templateFolder = new DirectoryInfo(Path.Combine(BehaviourEngine.AssemblyDirectory.FullName, "Pandora_Engine\\Skyrim\\Template"));

	private static readonly DirectoryInfo defaultOutputMeshFolder = new DirectoryInfo(Path.Join(Directory.GetCurrentDirectory(), "meshes"));

	private static readonly DirectoryInfo currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
	public ProjectManager ProjectManager { get; private set; }
	public AnimDataManager AnimDataManager { get; private set; }
	public AnimSetDataManager AnimSetDataManager { get; private set; }

	private PandoraFragmentInterpreter pandoraConverter;

	private IMetaDataExporter<PackFile> exporter = new PackFileExporter();
	public NemesisAssembler()
    {
		ProjectManager = new ProjectManager(templateFolder, currentDirectory);
		AnimSetDataManager = new AnimSetDataManager(templateFolder, defaultOutputMeshFolder);
		AnimDataManager = new AnimDataManager(templateFolder, defaultOutputMeshFolder);

		pandoraConverter = new PandoraFragmentInterpreter(ProjectManager, AnimSetDataManager, AnimDataManager);
    }
	public NemesisAssembler(IMetaDataExporter<PackFile> ioManager)
	{
		this.exporter = ioManager;
		ProjectManager = new ProjectManager(templateFolder, currentDirectory);
		AnimSetDataManager = new AnimSetDataManager(templateFolder, defaultOutputMeshFolder);
		AnimDataManager = new AnimDataManager(templateFolder, defaultOutputMeshFolder);

		pandoraConverter = new PandoraFragmentInterpreter(ProjectManager, AnimSetDataManager, AnimDataManager);
	}
	public NemesisAssembler(IMetaDataExporter<PackFile> ioManager, ProjectManager projManager, AnimSetDataManager animSDManager, AnimDataManager animDManager)
	{
		this.exporter = ioManager;
		this.ProjectManager = projManager;
		this.AnimSetDataManager = animSDManager;
		this.AnimDataManager = animDManager;
		pandoraConverter = new PandoraFragmentInterpreter(ProjectManager, AnimSetDataManager, AnimDataManager);
	}

	public void SetOutputPath(DirectoryInfo baseOutputDirectory)
	{
		var outputMeshDirectory = new DirectoryInfo(Path.Join(baseOutputDirectory.FullName, "meshes")); 
		ProjectManager.SetOutputPath(baseOutputDirectory);
		AnimDataManager.SetOutputPath(outputMeshDirectory);
		AnimSetDataManager.SetOutputPath(outputMeshDirectory);
	}

	public void LoadResources()
	{
		throw new NotImplementedException();


	}
	public void GetPostMessages(StringBuilder builder)
	{
		ProjectManager.GetFNISInfo(builder);
		ProjectManager.GetAnimationInfo(builder);
		ProjectManager.GetExportInfo(builder);
	}
	public async Task LoadResourcesAsync()
	{
		var animSetDataTask = Task.Run(() => { AnimSetDataManager.SplitAnimSetDataSingleFile(); });
		await Task.Run(ProjectManager.LoadTrackedProjects);
		await Task.Run(() => { AnimDataManager.SplitAnimationDataSingleFile(ProjectManager); });
		await animSetDataTask; 

	}

	public void AssemblePatch(IModInfo modInfo)
	{
		DirectoryInfo folder = modInfo.Folder;
		DirectoryInfo[] subFolders = folder.GetDirectories();

		foreach (DirectoryInfo subFolder in subFolders)
		{
			if (AssemblePackFilePatch(subFolder, modInfo)) continue;
			if (subFolder.Name.StartsWith("animationsetdata"))
			{
				AssembleAnimSetDataPatch(subFolder);
				continue; 
			} 
			if (subFolder.Name.StartsWith("animationdata"))
			{
				AssembleAnimDataPatch(subFolder);
				continue;
			}
			if (subFolder.Name.StartsWith("plugin"))
			{
				
			}

		}
	}
	public void AssemblePatch(IModInfo modInfo, DirectoryInfo folder)
	{
		DirectoryInfo[] subFolders = folder.GetDirectories();
		foreach (DirectoryInfo subFolder in subFolders)
		{
			if (AssemblePackFilePatch(subFolder, modInfo)) continue;
			if (subFolder.Name.StartsWith("animationsetdata")) AssembleAnimSetDataPatch(subFolder);
			if (subFolder.Name.StartsWith("animationdata")) AssembleAnimDataPatch(subFolder);

		}
	}

	public bool ApplyPatches() => true;

	public async Task<bool> ApplyPatchesAsync()
	{
		var loadMetaDataTask = Task.Run(() => { exporter.LoadMetaData(); });

		
		var mainTask = await Task.Run<bool>(() => { return ProjectManager.ApplyPatchesParallel(); });
		await Task.Run(() => { pandoraConverter.ApplyNativePatches(); });
		var animSetDataTask = Task.Run(() => { AnimSetDataManager.MergeAnimSetDataSingleFile(); });
		var animDataTask = Task.Run(() => { AnimDataManager.MergeAnimDataSingleFile(); });
		await loadMetaDataTask;
		
		bool exportSuccess = exporter.ExportParallel(ProjectManager.ActivePackFiles);
		var saveMetaDataTask = Task.Run(() => { exporter.SaveMetaData(ProjectManager.ActivePackFiles); });
		await animDataTask;
		await animSetDataTask;
		await saveMetaDataTask;
		return mainTask && exportSuccess;
	}

    public void ForwardReplaceEdit(PackFile packFile, string nodeName,XMatch match, PackFileChangeSet changeSet, XPathLookup lookup)
    {
        List<XNode> newNodes = new List<XNode>();
        int separatorIndex = match.Count;

		XNode? previousNode = match[0].PreviousNode;
		
		for (int i = 1; i < separatorIndex; i++)
        {

            XNode node = match[i];
			
            if (node.NodeType == XmlNodeType.Comment)
            {
                separatorIndex = i;
                break;
            }
            newNodes.Add(node);
        }

		if (newNodes.Count > 0)
        {
			for (int i = separatorIndex + 1; i < match.Count - 1; i++)
			{
				XNode node = match[i];
				XNode newNode = newNodes[i - separatorIndex - 1];
				switch (node.NodeType)
				{
					case XmlNodeType.Text:
						
						StringBuilder previousTextBuilder = new ();
						StringBuilder bufferTextBuilder = new(); 
						bool skipText = false;
						previousNode = newNode.PreviousNode?.PreviousNode;
						while (previousNode != null)
						{
							if (previousNode.NodeType == XmlNodeType.Comment)
							{
								var comment = (XComment)previousNode;
								if (comment.Value.Contains("close", StringComparison.OrdinalIgnoreCase))
								{
									skipText = true;
								}
								else if (comment.Value.Contains("open", StringComparison.OrdinalIgnoreCase))
								{
									skipText = false;
									previousTextBuilder.Insert(0, bufferTextBuilder);
									bufferTextBuilder = bufferTextBuilder.Clear();
								}
								else if (comment.Value.Contains("original", StringComparison.OrdinalIgnoreCase))
								{
									skipText = false; 
									bufferTextBuilder = bufferTextBuilder.Clear();
								}
									previousNode = previousNode.PreviousNode;
									continue;
							}
							if (skipText) 
							{
								bufferTextBuilder.Insert(0, '\n');
								bufferTextBuilder.Insert(0, previousNode.ToString());
								previousNode = previousNode.PreviousNode;
								continue; 
							}
							previousTextBuilder.Insert(0, '\n');
							previousTextBuilder.Insert(0,previousNode.ToString());
							previousNode = previousNode.PreviousNode;
						}

						string preText = previousTextBuilder.ToString();
						string oldText = ((XText)node).Value;
						string newText = ((XText)newNode).Value;
						//packFile.Editor.QueueReplaceText(lookup.LookupPath(node), ((XText)node).Value, ((XText)newNodes[i - separatorIndex - 1]).Value);

						changeSet.AddChange(new ReplaceTextChange(nodeName, lookup.LookupPath(node), preText, oldText, newText));
						//lock (packFile.edits) packFile.edits.AddChange(new ReplaceTextChange(lookup.LookupPath(node), ((XText)node).Value, ((XText)newNodes[i - separatorIndex - 1]).Value,modInfo));
						break;
						

					case XmlNodeType.Element:
						//packFile.Editor.QueueReplaceElement(lookup.LookupPath(node), (XElement)newNodes[i - separatorIndex - 1]);
						changeSet.AddChange(new ReplaceElementChange(nodeName,lookup.LookupPath(newNode), (XElement)newNode));
						//lock (packFile.edits) packFile.edits.AddChange(new ReplaceElementChange(lookup.LookupPath(node), (XElement)newNodes[i - separatorIndex - 1],modInfo));
						break;
					default:
						break;
				}
			}
            return; 
		}
		for (int i = separatorIndex + 1; i < match.Count - 1; i++)
		{
			XNode node = match[i];
			switch (node.NodeType)
			{
				case XmlNodeType.Text:
					break;
				case XmlNodeType.Element:
					//packFile.Editor.QueueRemoveElement(lookup.LookupPath(node));
					changeSet.AddChange(new RemoveElementChange(nodeName,lookup.LookupPath(node)));
					//lock (packFile.edits) packFile.edits.AddChange(new RemoveElementChange(lookup.LookupPath(node),modInfo));
					break;
				default:
					break;
			}
		}

	}

    public void ForwardInsertEdit(PackFile packFile,string nodeName, XMatch match, PackFileChangeSet changeSet, XPathLookup lookup)
    {
        List<XNode> newNodes = match.nodes;
		XNode? previousNode;
		XNode? nextNode = newNodes.Last().NextNode;
		

		newNodes.RemoveAt(0);
		newNodes.RemoveAt(newNodes.Count - 1);
		bool isTextInsert = nextNode != null && nextNode.NodeType == XmlNodeType.Text;


		foreach (XNode node in newNodes)
        {
			string nodePath = lookup.LookupPath(node);
			//if (node.Parent != null) node.Remove();
			switch (node.NodeType)
            {
                case XmlNodeType.Text:

					

					//packFile.Editor.QueueInsertText(lookup.LookupPath(node), ((XText)node).Value);
					if (!isTextInsert) 
					{
						changeSet.AddChange(new AppendTextChange(nodeName,nodePath, ((XText)node).Value));
						break;
					}

					StringBuilder previousTextBuilder = new();
					StringBuilder bufferTextBuilder = new();
					bool skipText = false;
					previousNode = node.PreviousNode?.PreviousNode;
					while (previousNode != null)
					{
						if (previousNode.NodeType == XmlNodeType.Comment)
						{
							var comment = (XComment)previousNode;
							if (comment.Value.Contains("close", StringComparison.OrdinalIgnoreCase))
							{
								skipText = true;
							}
							else if (comment.Value.Contains("open", StringComparison.OrdinalIgnoreCase))
							{
								skipText = false;
								previousTextBuilder.Insert(0, bufferTextBuilder);
								bufferTextBuilder = bufferTextBuilder.Clear();
							}
							else if (comment.Value.Contains("original", StringComparison.OrdinalIgnoreCase))
							{
								skipText = false;
								bufferTextBuilder = bufferTextBuilder.Clear();
							}
							previousNode = previousNode.PreviousNode;
							continue;
						}
						if (skipText)
						{
							bufferTextBuilder.Insert(0, '\n');
							bufferTextBuilder.Insert(0, previousNode.ToString());
							previousNode = previousNode.PreviousNode;
							continue;
						}
						previousTextBuilder.Insert(0, '\n');
						previousTextBuilder.Insert(0, previousNode.ToString());
						previousNode = previousNode.PreviousNode;
					}

					string preText = previousTextBuilder.ToString();
					changeSet.AddChange(new InsertTextChange(nodeName, nodePath, preText, ((XText)node).Value));

					//lock (packFile.edits) packFile.edits.AddChange(new InsertTextChange(nodePath, ((XText)node).Value, modInfo));
                    break; 
                case XmlNodeType.Element:
					//packFile.Editor.QueueInsertElement(lookup.LookupPath(node), (XElement)node);
					lock (packFile.Dispatcher)
					{
						if (packFile.Map.PathExists(nodePath))
						{
							changeSet.AddChange(new InsertElementChange(nodeName,nodePath, (XElement)node));
							//packFile.edits.AddChange(new InsertElementChange(nodePath, (XElement)node, modInfo));
						}
						else
						{
							changeSet.AddChange(new AppendElementChange(nodeName,nodePath.Substring(0, nodePath.LastIndexOf('/')), (XElement)node));
							//packFile.edits.AddChange(new AppendElementChange(nodePath.Substring(0, nodePath.LastIndexOf('/')), (XElement)node, modInfo));
						}
					}
					break;
				default:
					break;
            }
        }
	}

    public bool MatchReplacePattern(PackFile packFile, string nodeName,List<XNode> nodes, PackFileChangeSet changeSet, XPathLookup lookup)
    {
        XMatchCollection matchCollection = replacePattern.Matches(nodes);
        if (!matchCollection.Success) return false;
        foreach(XMatch match in matchCollection)
        {
            ForwardReplaceEdit(packFile, nodeName,match, changeSet, lookup);
        }
        return true;
    }

    public bool MatchInsertPattern(PackFile packFile, string nodeName, List<XNode> nodes, PackFileChangeSet changeSet, XPathLookup lookup)
    {
		XMatchCollection matchCollection = insertPattern.Matches(nodes);
		if (!matchCollection.Success) return false;
		foreach(XMatch match in matchCollection)
        {
            ForwardInsertEdit(packFile, nodeName,match, changeSet, lookup); 
        }
		return true;
	}
	

	public void AssembleAnimDataPatch(DirectoryInfo folder)
	{
		foreach(var subFolder in folder.GetDirectories())
		{
			pandoraConverter.TryGenerateAnimDataPatchFile(subFolder);
		}
		pandoraConverter.AssembleAnimDataPatch(folder);
	}
	public void AssembleAnimSetDataPatch(DirectoryInfo directoryInfo)
	{
		pandoraConverter.AssembleAnimSetDataPatch(directoryInfo);
	}
	private PackFileChangeSet AssemblePackFileChanges(PackFile packFile, IModInfo modInfo, DirectoryInfo folder)
	{
		FileInfo[] editFiles = folder.GetFiles("#*.txt");
		
		var changeSet = new PackFileChangeSet(modInfo);
		var modName = modInfo.Name;
		XPathLookup lookup = new XPathLookup();
		foreach (FileInfo editFile in editFiles)
		{
			List<XNode> nodes = new List<XNode>();
			string nodeName = Path.GetFileNameWithoutExtension(editFile.Name);
			XElement element;
			try
			{
				element = XElement.Load(editFile.FullName);
			}
			catch (XmlException e)
			{
				Logger.Error($"Nemesis Assembler > File {editFile.FullName} > Load > FAILED > {e.Message}");
				continue;
			}
			nodes = lookup.MapFromElement(element);

			lock (packFile)
			{
				if (!packFile.PopObjectAsXml(nodeName))
				{
					packFile.Dispatcher.TrackPotentialNode(packFile, nodeName, element);
				}
			}
			MatchInsertPattern(packFile, nodeName, nodes, changeSet, lookup);
			MatchReplacePattern(packFile, nodeName, nodes, changeSet, lookup);

			//if (!hasReplacements && !hasInserts)
			//{
			//	if (packFile.Map.PathExists(modName))
			//	{
			//		Logger.Error($"Nemesis Assembler > File {editFile.FullName} >  No Edits Found > Load > SKIPPED");
			//		continue;
			//	}
			//	changeSet.AddChange(new PushElementChange(PackFile.ROOT_CONTAINER_NAME, element));
			//}
		}
		return changeSet;
	}
	private bool AssemblePackFilePatch(DirectoryInfo folder,Project project,IModInfo modInfo)
	{
		PackFile targetPackFile;
		if (!ProjectManager.TryActivatePackFilePriority(folder.Name, project, out targetPackFile!))
		{
			return false;
		}
		lock (targetPackFile) targetPackFile.Dispatcher.AddChangeSet(AssemblePackFileChanges(targetPackFile, modInfo, folder));
		return true;
	}
	private bool AssemblePackFilePatch(DirectoryInfo folder, IModInfo modInfo)
    {
		PackFile targetPackFile; 
		if (!ProjectManager.TryActivatePackFilePriority(folder.Name, out targetPackFile!))
		{
			Project targetProject;
			if (!ProjectManager.TryLookupProjectFolder(folder.Name, out targetProject!)) {  return false; }


			DirectoryInfo[] subFolders = folder.GetDirectories();
			foreach(DirectoryInfo subFolder in subFolders) 
			{
				AssemblePackFilePatch(subFolder, targetProject, modInfo);
			}
			return true;
		}
		lock (targetPackFile) targetPackFile.Dispatcher.AddChangeSet(AssemblePackFileChanges(targetPackFile, modInfo, folder));
		return true;
	}

	public List<(FileInfo inFile, FileInfo outFile)> GetExportFiles()
	{
		List < (FileInfo inFile, FileInfo outFile) > exportFiles = new List<(FileInfo inFile, FileInfo outFile)> ();
		foreach (PackFile packFile in ProjectManager.ActivePackFiles)
        {
            exportFiles.Add((packFile.InputHandle, new FileInfo(Path.Join(Directory.GetCurrentDirectory(), packFile.InputHandle.Name)))); 
        }

        return exportFiles; 
	}


}
