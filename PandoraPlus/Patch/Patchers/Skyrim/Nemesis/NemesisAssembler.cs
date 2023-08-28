using Pandora.Core.Patchers.Skyrim;
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

    private IXExpression replacePattern = new XWrapExpression(new XStep(XmlNodeType.Comment, "OPEN"), new XStep(XmlNodeType.Comment, "ORIGINAL"), new XStep(XmlNodeType.Comment, "CLOSE"));
    private IXExpression insertPattern = new XWrapExpression(new XStep(XmlNodeType.Comment, "OPEN"), new XStep(XmlNodeType.Comment, "CLOSE")); 

    private XPathLookup lookup = new XPathLookup();
    
    List<PackFile> packFiles = new List<PackFile>();

    private ProjectManager projectManager = new ProjectManager();

    

	public void LoadResources()
	{
		projectManager.LoadProject("actors\\character\\defaultmale.hkx");
		projectManager.LoadProject("actors\\character\\defaultfemale.hkx");

	}
	public void AssemblePatch(DirectoryInfo folder)
	{
		DirectoryInfo[] subFolders = folder.GetDirectories();

		foreach (DirectoryInfo subFolder in subFolders)
		{
			AssemblePackFilePatch(subFolder);
		}
	}
    
	public void ApplyPatches()
	{
        foreach(PackFile packFile in projectManager.ActivePackFiles)
        {
			packFile.ApplyChanges();
			packFile.Map.Save(Path.Join(Directory.GetCurrentDirectory(), packFile.Handle.Name));
        }
	}
    public void ForwardReplaceEdit(PackFile packFile, XMatch match)
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
						packFile.Editor.QueueReplaceText(lookup.LookupPath(node), ((XText)node).Value, ((XText)newNodes[i - separatorIndex - 1]).Value);
						break;
					case XmlNodeType.Element:
						packFile.Editor.QueueReplaceElement(lookup.LookupPath(node), (XElement)newNodes[i - separatorIndex - 1]);
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
					packFile.Editor.QueueRemoveText(lookup.LookupPath(node), ((XText)node).Value);
					break;
				case XmlNodeType.Element:
					packFile.Editor.QueueRemoveElement(lookup.LookupPath(node));
					break;
				default:
					break;
			}
		}

	}

    public void ForwardInsertEdit(PackFile packFile, XMatch match)
    {
        List<XNode> newNodes = match.nodes;
		newNodes.RemoveAt(0);
		newNodes.RemoveAt(newNodes.Count - 1);
        
        foreach(XNode node in newNodes)
        {
            switch(node.NodeType)
            {
                case XmlNodeType.Text:
                    packFile.Editor.QueueInsertText(lookup.LookupPath(node), ((XText)node).Value);
                    break; 
                case XmlNodeType.Element:
                    packFile.Editor.QueueInsertElement(lookup.LookupPath(node), (XElement)node);
                    break;
                default:
                    break;
            }
        }
	}

    public bool MatchReplacePattern(PackFile packFile, List<XNode> nodes)
    {
        XMatchCollection matchCollection = replacePattern.Matches(nodes);
        if (!matchCollection.Success) return false;
        foreach(XMatch match in matchCollection)
        {
            ForwardReplaceEdit(packFile, match);
        }
        return true;
    }

    public bool MatchInsertPattern(PackFile packFile, List<XNode> nodes)
    {
		XMatchCollection matchCollection = insertPattern.Matches(nodes);
		if (!matchCollection.Success) return false;
		foreach(XMatch match in matchCollection)
        {
            ForwardInsertEdit(packFile, match); 
        }
		return true;
	}



	private void AssemblePackFilePatch(DirectoryInfo folder)
    {
		if (!projectManager.ContainsPackFile(folder.Name)) return;

		var targetPackFile = projectManager.ActivatePackFile(folder.Name);

		FileInfo[] editFiles = folder.GetFiles("#*.txt");

		foreach (FileInfo editFile in editFiles)
		{
			XElement element = XElement.Load(editFile.FullName);
			List<XNode> nodes = lookup.MapFromElement(element);
            targetPackFile.MapNode(Path.GetFileNameWithoutExtension(editFile.Name));

			if (!MatchReplacePattern(targetPackFile, nodes) && !MatchInsertPattern(targetPackFile, nodes))
            {
                targetPackFile.TopLevelInsertElements.Add(element);
            }
		}
	}

	public List<(FileInfo inFile, FileInfo outFile)> GetExportFiles()
	{
		List < (FileInfo inFile, FileInfo outFile) > exportFiles = new List<(FileInfo inFile, FileInfo outFile)> ();
		foreach (PackFile packFile in projectManager.ActivePackFiles)
        {
            exportFiles.Add((packFile.Handle, new FileInfo(Path.Join(Directory.GetCurrentDirectory(), packFile.Handle.Name)))); 
        }

        return exportFiles; 
	}
}
