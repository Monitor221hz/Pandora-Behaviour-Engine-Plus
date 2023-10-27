using NLog;
using Pandora.Core;
using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.AnimData;
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


	private IXExpression replacePattern = new XWrapExpression(new XStep(XmlNodeType.Comment, "OPEN"), new XStep(XmlNodeType.Comment, "ORIGINAL"), new XStep(XmlNodeType.Comment, "CLOSE"));
    private IXExpression insertPattern = new XSkipWrapExpression(new XStep(XmlNodeType.Comment, "ORIGINAL"), new XStep(XmlNodeType.Comment, "OPEN"), new XStep(XmlNodeType.Comment, "CLOSE")); 

    private XPathLookup lookup = new XPathLookup();
    
    List<PackFile> packFiles = new List<PackFile>();

    private ProjectManager projectManager = new ProjectManager();

	private AnimDataPatcher animDataPatcher = new AnimDataPatcher();

    public void LoadResources()
	{
		
		projectManager.LoadProject("actors\\character\\defaultmale.hkx");
		projectManager.LoadProject("actors\\character\\defaultfemale.hkx");
		animDataPatcher.SplitAnimationDataSingleFile(projectManager);
	}
	public void AssemblePatch(IModInfo mod)
	{
		DirectoryInfo folder = mod.Folder;
		DirectoryInfo[] subFolders = folder.GetDirectories();

		foreach (DirectoryInfo subFolder in subFolders)
		{
			AssemblePackFilePatch(subFolder,mod.Name);
		}
	}
    
	public void ApplyPatches()
	{
        foreach(PackFile packFile in projectManager.ActivePackFiles)
        {
			packFile.ApplyChanges();
			//packFile.Map.Save(Path.Join(Directory.GetCurrentDirectory(), packFile.InputHandle.Name));
			packFile.Export();
        }
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
            switch(node.NodeType)
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



	private void AssemblePackFilePatch(DirectoryInfo folder, string modName)
    {
		if (!projectManager.ContainsPackFile(folder.Name)) return;

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
				Logger.Error($"File {editFile.FullName} > Load > FAILED > {e.Message}");
				continue;
			}
			
			lock(lookup)
			{
				nodes = lookup.MapFromElement(element);
			}
            targetPackFile.MapNode(nodeName);
			bool hasReplacements = MatchReplacePattern(targetPackFile, nodes, modName);
			bool hasInserts = MatchInsertPattern(targetPackFile, nodes, modName);
			if (!hasReplacements && !hasInserts)
            {
				targetPackFile.edits.AddChange(new InsertElementChange(PackFile.ROOT_CONTAINER_NAME+"/top", element, modName));
            }
		}
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
