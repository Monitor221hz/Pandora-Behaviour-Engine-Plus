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

    List<XMatchCollection> replaceMatchGroups = new List<XMatchCollection>();

    List<XMatchCollection> insertMatchGroups = new List<XMatchCollection>();
    
    List<PackFile> packFiles = new List<PackFile>();

    private ProjectManager projectManager = new ProjectManager();
    
    public void Apply(PackFile packFile)
    {
        foreach (var matchGroup in replaceMatchGroups)
        {
            foreach (var match in matchGroup)
            {

            }
        }
    }


    public void Replace(XMatch match)
    {
        List<XNode> newNodes = new List<XNode>();
        List<XNode> originalNodes = new List<XNode>();
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
        for (int i = separatorIndex + 1; i < match.Count - 1; i++)
        {
            XNode node = match[i];
            originalNodes.Add(node);
        }
    }

    public bool MatchReplacePattern(XElement element)
    {
        
        var nodes = lookup.MapFromElement(element);
        XMatchCollection matchCollection = replacePattern.Matches(nodes);
        if (!matchCollection.Success) return false;
        replaceMatchGroups.Add(matchCollection);
        return true;
    }

    public bool MatchInsertPattern(XElement element)
    {
		
		var nodes = lookup.MapFromElement(element);
		XMatchCollection matchCollection = replacePattern.Matches(nodes);
		if (!matchCollection.Success) return false;
		replaceMatchGroups.Add(matchCollection);
		return true;
	}

	public void AssemblePatch(DirectoryInfo folder)
	{
        if (!projectManager.ContainsPackFile(folder.Name)) return;

        var targetPackFile = projectManager.LookupPackFile(folder.Name);

        FileInfo[] editFiles = folder.GetFiles("#*.txt");

        foreach (FileInfo editFile in editFiles )
        {
            XElement element = XElement.Load(editFile.FullName); 
            if (!MatchReplacePattern(element)) MatchInsertPattern(element);
        }
	}

	public void LoadResources()
	{
		projectManager.LoadProject("actors\\character\\defaultmale.hkx");
		projectManager.LoadProject("actors\\character\\defaultfemale.hkx");
	}
}
