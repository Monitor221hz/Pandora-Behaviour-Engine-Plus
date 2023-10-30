using NLog;
using Pandora.Core;
using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.AnimData;
using Pandora.Patch.Patchers.Skyrim.AnimSetData;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Linq;
using XmlCake.Linq;
using XmlCake.Linq.Expressions;

namespace Pandora.Patch.Patchers.Skyrim.Nemesis;

public class NemesisAssembler : IAssembler
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger(); //to do: move logger into inheritable base class


	private IXExpression replacePattern = new XSkipWrapExpression(new XStep(XmlNodeType.Comment, "CLOSE"),new XStep(XmlNodeType.Comment, "OPEN"), new XStep(XmlNodeType.Comment, "ORIGINAL"), new XStep(XmlNodeType.Comment, "CLOSE"));
    private IXExpression insertPattern = new XSkipWrapExpression(new XStep(XmlNodeType.Comment, "ORIGINAL"), new XStep(XmlNodeType.Comment, "OPEN"), new XStep(XmlNodeType.Comment, "CLOSE")); 

    private XPathLookup lookup = new XPathLookup();
    
    List<PackFile> packFiles = new List<PackFile>();

	private DirectoryInfo currentFolder = new DirectoryInfo(Directory.GetCurrentDirectory());

	private DirectoryInfo templateFolder = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Pandora_Engine\\Skyrim\\Template");

	private DirectoryInfo outputFolder = new DirectoryInfo($"{Directory.GetCurrentDirectory()}\\meshes");

	private ProjectManager projectManager;

	private AnimSetDataManager animSetDataManager;

    public NemesisAssembler()
    {
		projectManager = new ProjectManager(templateFolder, outputFolder);
		animSetDataManager = new AnimSetDataManager(templateFolder, outputFolder);
    }

    public void LoadResources()
	{
		projectManager.LoadTrackedProjects();
		projectManager.LoadAnimData();

	}

	public async Task LoadResourcesAsync()
	{
		var animSetDataTask = Task.Run(() => { animSetDataManager.SplitAnimSetDataSingleFile(); });
		projectManager.LoadTrackedProjects();
		await Task.Run(() => { projectManager.LoadAnimData(); });
		await animSetDataTask; 

	}

	public void AssemblePatch(IModInfo mod)
	{
		DirectoryInfo folder = mod.Folder;
		DirectoryInfo[] subFolders = folder.GetDirectories();

		foreach (DirectoryInfo subFolder in subFolders)
		{
			if (AssemblePackFilePatch(subFolder, mod.Name) || subFolder.Name != "animsetdatasinglefile") continue;
			AssembleAnimSetDataPatch(subFolder);

		}
	}
    
	public void ApplyPatches() => projectManager.ApplyPatches();

	public async Task ApplyPatchesAsync()
	{
		var animSetDataTask = Task.Run(() => { animSetDataManager.MergeAnimSetDataSingleFile(); });
		await projectManager.ApplyPatchesAsync();
		await animSetDataTask;
	}

	//to-fix: certain excerpts being misclassified as single replace edit when it is actually a replace and insert edit

	//
    public void ForwardReplaceEdit(PackFile packFile, XMatch match, string modName)
    {
        List<XNode> newNodes = new List<XNode>();
        int separatorIndex = match.Count;
        for (int i = 1; i < separatorIndex; i++)
        {

            XNode node = match[i];
            if (node.NodeType == XmlNodeType.Comment)
            {
                separatorIndex = i;
                break;
            }
            newNodes.Add(node);
			//if (node.Parent != null) node.Remove();

        }
        if (newNodes.Count > 0)
        {
			for (int i = separatorIndex + 1; i < match.Count - 1; i++)
			{
				XNode node = match[i];


				switch (node.NodeType)
				{
					case XmlNodeType.Text:
						//packFile.Editor.QueueReplaceText(lookup.LookupPath(node), ((XText)node).Value, ((XText)newNodes[i - separatorIndex - 1]).Value);
						lock (packFile.edits) packFile.edits.AddChange(new ReplaceTextChange(lookup.LookupPath(node), ((XText)node).Value, ((XText)newNodes[i - separatorIndex - 1]).Value,modName));
						break;
					case XmlNodeType.Element:
						//packFile.Editor.QueueReplaceElement(lookup.LookupPath(node), (XElement)newNodes[i - separatorIndex - 1]);
						lock (packFile.edits) packFile.edits.AddChange(new ReplaceElementChange(lookup.LookupPath(node), (XElement)newNodes[i - separatorIndex - 1],modName));
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
					//packFile.Editor.QueueRemoveText(lookup.LookupPath(node), ((XText)node).Value);
					lock (packFile.edits) packFile.edits.AddChange(new RemoveTextChange(lookup.LookupPath(node), ((XText)node).Value, modName));
					break;
				case XmlNodeType.Element:
					//packFile.Editor.QueueRemoveElement(lookup.LookupPath(node));
					lock (packFile.edits) packFile.edits.AddChange(new RemoveElementChange(lookup.LookupPath(node),modName));
					break;
				default:
					break;
			}
		}

	}

    public void ForwardInsertEdit(PackFile packFile, XMatch match, string modName)
    {
        List<XNode> newNodes = match.nodes;
		newNodes.RemoveAt(0);
		newNodes.RemoveAt(newNodes.Count - 1);
        
        foreach(XNode node in newNodes)
        {
			string nodePath = lookup.LookupPath(node);
			//if (node.Parent != null) node.Remove();
			switch (node.NodeType)
            {
                case XmlNodeType.Text:
					//packFile.Editor.QueueInsertText(lookup.LookupPath(node), ((XText)node).Value);
					lock (packFile.edits) packFile.edits.AddChange(new InsertTextChange(nodePath, ((XText)node).Value, modName));
                    break; 
                case XmlNodeType.Element:
					//packFile.Editor.QueueInsertElement(lookup.LookupPath(node), (XElement)node);
					lock (packFile.edits)
					{
						if (packFile.Map.PathExists(nodePath))
						{
							packFile.edits.AddChange(new InsertElementChange(nodePath, (XElement)node, modName));
						}
						else
						{
							packFile.edits.AddChange(new AppendElementChange(nodePath.Substring(0, nodePath.LastIndexOf('/')), (XElement)node, modName));
						}
					}
					break;
				default:
					break;
            }
        }
	}

    public bool MatchReplacePattern(PackFile packFile, List<XNode> nodes, string modName)
    {
        XMatchCollection matchCollection = replacePattern.Matches(nodes);
        if (!matchCollection.Success) return false;
        foreach(XMatch match in matchCollection)
        {
            ForwardReplaceEdit(packFile, match, modName);
        }
        return true;
    }

    public bool MatchInsertPattern(PackFile packFile, List<XNode> nodes, string modName)
    {
		XMatchCollection matchCollection = insertPattern.Matches(nodes);
		if (!matchCollection.Success) return false;
		foreach(XMatch match in matchCollection)
        {
            ForwardInsertEdit(packFile, match, modName); 
        }
		return true;
	}

	public void AssembleAnimSetDataPatch(DirectoryInfo directoryInfo) //technically not Nemesis format but this format is just simpler and
	{
		ProjectAnimSetData? targetAnimSetData;
		foreach(DirectoryInfo subDirInfo in directoryInfo.GetDirectories())
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

	private bool AssemblePackFilePatch(DirectoryInfo folder, string modName)
    {
		if (!projectManager.ContainsPackFile(folder.Name)) return false;

		var targetPackFile = projectManager.ActivatePackFile(folder.Name);

		FileInfo[] editFiles = folder.GetFiles("#*.txt");

		foreach (FileInfo editFile in editFiles)
		{
			List<XNode> nodes =new List<XNode>();
			string nodeName = Path.GetFileNameWithoutExtension(editFile.Name);
			XElement element;
			try
			{
				element = XElement.Load(editFile.FullName);
			}
			catch(XmlException e)
			{
				Logger.Error($"Nemesis Assembler > File {editFile.FullName} > Load > FAILED > {e.Message}");
				continue;
			}
			
			lock(lookup)
			{
				nodes = lookup.MapFromElement(element);
			}
            targetPackFile.MapNode(nodeName);
			bool hasInserts = MatchInsertPattern(targetPackFile, nodes, modName);
			bool hasReplacements = MatchReplacePattern(targetPackFile, nodes, modName);

			if (!hasReplacements && !hasInserts)
            {
				if (targetPackFile.Map.PathExists(modName))
				{
					Logger.Error($"Nemesis Assembler > File {editFile.FullName} >  No Edits Found > Load > SKIPPED");
					continue;
				}
				targetPackFile.edits.AddChange(new InsertElementChange(PackFile.ROOT_CONTAINER_NAME+"/top", element, modName));
            }
		}
		return true;
	}

	public List<(FileInfo inFile, FileInfo outFile)> GetExportFiles()
	{
		List < (FileInfo inFile, FileInfo outFile) > exportFiles = new List<(FileInfo inFile, FileInfo outFile)> ();
		foreach (PackFile packFile in projectManager.ActivePackFiles)
        {
            exportFiles.Add((packFile.InputHandle, new FileInfo(Path.Join(Directory.GetCurrentDirectory(), packFile.InputHandle.Name)))); 
        }

        return exportFiles; 
	}
}
