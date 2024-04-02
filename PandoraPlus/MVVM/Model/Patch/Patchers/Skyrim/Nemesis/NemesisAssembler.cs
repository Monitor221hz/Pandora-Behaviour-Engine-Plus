using NLog;
using Pandora.Core;
using Pandora.Core.IOManagers;
using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.IOManagers;
using Pandora.Patch.IOManagers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.AnimData;
using Pandora.Patch.Patchers.Skyrim.AnimSetData;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using Pandora.Patch.Patchers.Skyrim.Pandora;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Linq;
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

	private static readonly DirectoryInfo templateFolder = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Pandora_Engine\\Skyrim\\Template");

	private static readonly DirectoryInfo outputFolder = new DirectoryInfo($"{Directory.GetCurrentDirectory()}\\meshes");
	public ProjectManager ProjectManager { get; private set; }
	public AnimDataManager AnimDataManager { get; private set; }
	public AnimSetDataManager AnimSetDataManager { get; private set; }

	private PandoraConverter pandoraConverter;

	private Exporter<PackFile> exporter = new PackFileExporter();
	public NemesisAssembler()
    {



		ProjectManager = new ProjectManager(templateFolder, outputFolder);
		AnimSetDataManager = new AnimSetDataManager(templateFolder, outputFolder);
		AnimDataManager = new AnimDataManager(templateFolder, outputFolder);

		pandoraConverter = new PandoraConverter(ProjectManager, AnimSetDataManager, AnimDataManager);
    }
	public NemesisAssembler(Exporter<PackFile> ioManager)
	{
		this.exporter = ioManager;
		ProjectManager = new ProjectManager(templateFolder, outputFolder);
		AnimSetDataManager = new AnimSetDataManager(templateFolder, outputFolder);
		AnimDataManager = new AnimDataManager(templateFolder, outputFolder);

		pandoraConverter = new PandoraConverter(ProjectManager, AnimSetDataManager, AnimDataManager);
	}
	public NemesisAssembler(Exporter<PackFile> ioManager, ProjectManager projManager, AnimSetDataManager animSDManager, AnimDataManager animDManager)
	{
		this.exporter = ioManager;
		this.ProjectManager = projManager;
		this.AnimSetDataManager = animSDManager;
		this.AnimDataManager = animDManager;

		pandoraConverter = new PandoraConverter(ProjectManager, AnimSetDataManager, AnimDataManager);
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
			if (subFolder.Name == "animationsetdatasinglefile") AssembleAnimSetDataPatch(subFolder);
			if (subFolder.Name == "animationdatasinglefile") AssembleAnimDataPatch(subFolder);
			if (subFolder.Name == "animdata") subFolder.Delete(true);

		}
	}
	public void AssemblePatch(IModInfo modInfo, DirectoryInfo folder)
	{
		DirectoryInfo[] subFolders = folder.GetDirectories();
		foreach (DirectoryInfo subFolder in subFolders)
		{
			if (AssemblePackFilePatch(subFolder, modInfo)) continue;
			if (subFolder.Name == "animationsetdatasinglefile") AssembleAnimSetDataPatch(subFolder);
			if (subFolder.Name == "animationdatasinglefile") AssembleAnimDataPatch(subFolder);
			if (subFolder.Name == "animdata") subFolder.Delete(true);

		}
	}
    
	public bool ApplyPatches() => ProjectManager.ApplyPatches();

	public async Task<bool> ApplyPatchesAsync()
	{
		
		var animSetDataTask = Task.Run(() => { AnimSetDataManager.MergeAnimSetDataSingleFile(); });

		

		var animDataTask = Task.Run(() => { AnimDataManager.MergeAnimDataSingleFile(); });
		
		var mainTask = await ProjectManager.ApplyPatchesParallel();
		bool exportSuccess = exporter.ExportParallel(ProjectManager.ActivePackFiles);

		await animDataTask;
		await animSetDataTask;

		return mainTask && exportSuccess;

	}

	//to-fix: certain excerpts being misclassified as single replace edit when it is actually a replace and insert edit

	//
    public void ForwardReplaceEdit(PackFile packFile, XMatch match, PackFileChangeSet changeSet, XPathLookup lookup)
    {
        List<XNode> newNodes = new List<XNode>();
        int separatorIndex = match.Count;

		XNode? previousNode = match[0].PreviousNode;
		
		foreach(var node in match) { node.Remove();  }
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
						StringBuilder previousTextBuilder = new StringBuilder();

						while (previousNode != null && previousNode.NodeType == XmlNodeType.Text)
						{
							previousTextBuilder.Append(((XText)previousNode!).Value);
							previousNode = previousNode.PreviousNode;
						}

						string preText = previousTextBuilder.ToString();
						string oldText = ((XText)node).Value;
						string newText = ((XText)newNode).Value;
						//packFile.Editor.QueueReplaceText(lookup.LookupPath(node), ((XText)node).Value, ((XText)newNodes[i - separatorIndex - 1]).Value);

						changeSet.AddChange(new ReplaceTextChange(lookup.LookupPath(node), preText, oldText, newText));
						//lock (packFile.edits) packFile.edits.AddChange(new ReplaceTextChange(lookup.LookupPath(node), ((XText)node).Value, ((XText)newNodes[i - separatorIndex - 1]).Value,modInfo));
						break;
					case XmlNodeType.Element:
						//packFile.Editor.QueueReplaceElement(lookup.LookupPath(node), (XElement)newNodes[i - separatorIndex - 1]);
						changeSet.AddChange(new ReplaceElementChange(lookup.LookupPath(newNode), (XElement)newNode));
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
					StringBuilder previousTextBuilder = new StringBuilder();

					while (previousNode != null && previousNode.NodeType == XmlNodeType.Text)
					{
						previousTextBuilder.Append(((XText)previousNode!).Value);
						previousNode = previousNode.PreviousNode;
					}

					string preText = previousTextBuilder.ToString();
					string oldText = ((XText)node).Value;

					//packFile.Editor.QueueReplaceText(lookup.LookupPath(node), ((XText)node).Value, ((XText)newNodes[i - separatorIndex - 1]).Value);

					changeSet.AddChange(new ReplaceTextChange(lookup.LookupPath(node), preText, oldText, string.Empty));
					break;
				case XmlNodeType.Element:
					//packFile.Editor.QueueRemoveElement(lookup.LookupPath(node));
					changeSet.AddChange(new RemoveElementChange(lookup.LookupPath(node)));
					//lock (packFile.edits) packFile.edits.AddChange(new RemoveElementChange(lookup.LookupPath(node),modInfo));
					break;
				default:
					break;
			}
		}

	}

    public void ForwardInsertEdit(PackFile packFile, XMatch match, PackFileChangeSet changeSet, XPathLookup lookup)
    {
        List<XNode> newNodes = match.nodes;
		XNode? previousNode = newNodes.First().PreviousNode;
		XNode? nextNode = newNodes.Last().NextNode;
		

		foreach(var node in newNodes) { node.Remove(); }	
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
						changeSet.AddChange(new AppendTextChange(nodePath, ((XText)node).Value));
						break;
					} 

					StringBuilder previousTextBuilder = new StringBuilder();

					while (previousNode != null && previousNode.NodeType == XmlNodeType.Text)
					{
						previousTextBuilder.Append(((XText)previousNode!).Value);
						previousNode = previousNode.PreviousNode;
					}

					string preText = previousTextBuilder.ToString();
					changeSet.AddChange(new InsertTextChange(nodePath, preText, ((XText)node).Value));

					//lock (packFile.edits) packFile.edits.AddChange(new InsertTextChange(nodePath, ((XText)node).Value, modInfo));
                    break; 
                case XmlNodeType.Element:
					//packFile.Editor.QueueInsertElement(lookup.LookupPath(node), (XElement)node);
					lock (packFile.Dispatcher)
					{
						if (packFile.Map.PathExists(nodePath))
						{
							changeSet.AddChange(new InsertElementChange(nodePath, (XElement)node));
							//packFile.edits.AddChange(new InsertElementChange(nodePath, (XElement)node, modInfo));
						}
						else
						{
							changeSet.AddChange(new AppendElementChange(nodePath.Substring(0, nodePath.LastIndexOf('/')), (XElement)node));
							//packFile.edits.AddChange(new AppendElementChange(nodePath.Substring(0, nodePath.LastIndexOf('/')), (XElement)node, modInfo));
						}
					}
					break;
				default:
					break;
            }
        }
	}

    public bool MatchReplacePattern(PackFile packFile, List<XNode> nodes, PackFileChangeSet changeSet, XPathLookup lookup)
    {
        XMatchCollection matchCollection = replacePattern.Matches(nodes);
        if (!matchCollection.Success) return false;
        foreach(XMatch match in matchCollection)
        {
            ForwardReplaceEdit(packFile, match, changeSet, lookup);
        }
        return true;
    }

    public bool MatchInsertPattern(PackFile packFile, List<XNode> nodes, PackFileChangeSet changeSet, XPathLookup lookup)
    {
		XMatchCollection matchCollection = insertPattern.Matches(nodes);
		if (!matchCollection.Success) return false;
		foreach(XMatch match in matchCollection)
        {
            ForwardInsertEdit(packFile, match, changeSet, lookup); 
        }
		return true;
	}
	

	public void AssembleAnimDataPatch(DirectoryInfo folder)
	{
		foreach(var subFolder in folder.GetDirectories())
		{
			pandoraConverter.TryGenerateAnimDataPatchFile(subFolder);
		}
		pandoraConverter.Assembler.AssembleAnimDataPatch(folder);
	}
	public void AssembleAnimSetDataPatch(DirectoryInfo directoryInfo)
	{
		pandoraConverter.Assembler.AssembleAnimSetDataPatch(directoryInfo);
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

			lock (packFile) packFile.MapNode(nodeName);
			bool hasInserts = MatchInsertPattern(packFile, nodes, changeSet, lookup);
			bool hasReplacements = MatchReplacePattern(packFile, nodes, changeSet, lookup);

			if (!hasReplacements && !hasInserts)
			{
				if (packFile.Map.PathExists(modName))
				{
					Logger.Error($"Nemesis Assembler > File {editFile.FullName} >  No Edits Found > Load > SKIPPED");
					continue;
				}
				changeSet.AddChange(new PushElementChange(PackFile.ROOT_CONTAINER_NAME, element));
			}
		}
		return changeSet;
	}
	private bool AssemblePackFilePatch(DirectoryInfo folder,Project project,IModInfo modInfo)
	{
		PackFile targetPackFile;
		if (!ProjectManager.TryActivatePackFile(folder.Name, project, out targetPackFile!))
		{
			return false;
		}
		lock (targetPackFile) targetPackFile.Dispatcher.AddChangeSet(AssemblePackFileChanges(targetPackFile, modInfo, folder));
		return true;
	}
	private bool AssemblePackFilePatch(DirectoryInfo folder, IModInfo modInfo)
    {
		PackFile targetPackFile; 
		if (!ProjectManager.TryActivatePackFile(folder.Name, out targetPackFile!))
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
