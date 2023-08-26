using Pandora.Core.Patchers.Skyrim;
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

    List<XMatchCollection> matchGroups = new List<XMatchCollection>();

    private ProjectManager projectManager = new ProjectManager();
    
    public void Apply(PackFile packFile)
    {
        foreach (var matchGroup in matchGroups)
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

    public bool MatchForEdits(FileInfo file)
    {
        XElement element = XElement.Load(file.FullName);
        var nodes = lookup.MapFromElement(element);
        XMatchCollection matchCollection = replacePattern.Matches(nodes);
        if (!matchCollection.Success) return false;
        matchGroups.Add(matchCollection);
        return true;
    }

	public void AssemblePatch(DirectoryInfo folder)
	{
		projectManager.LookupFile(folder.Name)
	}

	public void LoadResources()
	{
		projectManager.LoadProject("actors\\character\\defaultmale.hkx");
		projectManager.LoadProject("actors\\character\\defaultfemale.hkx");
	}
}
