

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Xml;
using System.Xml.Linq;

namespace XmlCake.Linq; 

public class XPathTracker
{
    List<string> trackedPath = new List<string>();
    List<int> unnamedElementCounts = new List<int>();
    
    public XPathTracker()
    {
        GetPathName = GetPathNameFromAttribute;
        HasIdentifier = HasIdentiferFromAttribute;

        GetPathNameNode = GetPathNameFromAttribute;
        HasIdentifierNode = HasIdentifierFromAttribute; 
    }

    public void Reset()
    {

			trackedPath = new List<string>();
			unnamedElementCounts = new List<int>();
			maxDepth = -1;
			lastDepth = 2147483647;
            reset = true;

	}



    int maxDepth = -1;
    int lastDepth = 2147483647;

    private bool reset = true; 

    private Func<XmlReader, string> GetPathName;
    private Func<XmlReader, bool> HasIdentifier;

    private Func<XNode, string> GetPathNameNode;
    private Func<XNode, bool> HasIdentifierNode; 
    private string GetPathNameFromAttribute(XmlReader reader) =>  reader.GetAttribute(0);

    private string GetPathNameFromAttribute(XNode node)
    {
        XElement element = (XElement)node;

        return element.FirstAttribute!.Value; 
    }
    private bool HasIdentiferFromAttribute(XmlReader reader) => reader.HasAttributes;

    private bool HasIdentifierFromAttribute(XNode node)
    {
        if (node.NodeType != XmlNodeType.Element) return false;

        XElement element = (XElement)node;

        return element.HasAttributes; 
    }

    public string GetCurrentPath(XmlReader reader) => string.Join("/", trackedPath.SkipLast(maxDepth - reader.Depth));
	public string GetCurrentPathByElement(XNode node) => string.Join("/", trackedPath.SkipLast(Math.Abs(maxDepth - GetElementDepth(node))));


	//   public int GetNodeDepth(XNode node)
	//   {
	//       return (node.NodeType != XmlNodeType.Element) ? node.Ancestors().Count()+1 : node.Ancestors().Count();
	//}
	private int GetNodeDepthResolved(XNode node)
	{
		int depth = 0;
		XNode parentNode = node;
		while (parentNode.Parent != null)
		{
			parentNode = parentNode.Parent;
			depth++;
		}
		return depth;
	}

    private int GetElementDepth(XNode node)
    {
		int depth = 0;
		XNode parentNode = node;
		while (parentNode.Parent != null)
		{
			if (parentNode.NodeType == XmlNodeType.Element) { depth++; }
			parentNode = parentNode.Parent;
			
		}
		return depth;
	}
	private int GetNodeDepth(XNode node)
    {
        int depth = GetElementDepth(node);
        int ret = (node.NodeType == XmlNodeType.Text) ? depth : depth;
        return ret;
    }


	public void ResolvePath(XmlReader reader)
    {
        int depth = reader.Depth; 
        XmlNodeType nodeType = reader.NodeType;
		if (depth < lastDepth && depth + 1 < trackedPath.Count)  unnamedElementCounts[depth + 1] = 0;
        lastDepth = depth;
        if (depth > maxDepth)
        {
			ExtendPath(reader);
		}
        else if (depth < trackedPath.Count)
        {
            if (HasIdentifier(reader)) 
            {
                ChangePath(depth, GetPathName(reader));
                return;
            }
            if (IsContentNode(reader.NodeType))
            {
                trackedPath[depth] = nodeType.ToString() + unnamedElementCounts[depth];
                unnamedElementCounts[depth]++;
			}
            
        }
	}
    
    public void ResolvePath(XNode node)
    {
        reset = false; 
        int depth = GetNodeDepthResolved(node);
        XmlNodeType nodeType = node.NodeType;
		if (depth < lastDepth && depth + 1 < trackedPath.Count)
        {
            for (int i = depth+1; i < unnamedElementCounts.Count; i++)
            {
                unnamedElementCounts[i] = 0;
            }
			//unnamedElementCounts[depth + 1] = 0;
		}
		lastDepth = depth;
		if (depth > maxDepth && IsContentNode(nodeType))
		{
			ExtendPath(node, depth);
		}
		else if (depth < trackedPath.Count)
		{
			if (HasIdentifierNode(node))
			{
				ChangePath(depth, GetPathNameNode(node));
				return;
			}
			if (IsContentNode(nodeType))
			{
				trackedPath[depth] = nodeType.ToString() + unnamedElementCounts[depth];
				unnamedElementCounts[depth]++;
			}

		}
	}


	public static bool IsContentNode(XmlNodeType nodeType) => (XmlNodeType.EndElement != nodeType && XmlNodeType.Comment != nodeType);
    
    private void ChangePath(int depth, string value) => trackedPath[depth] = value;

    private void ExtendPath(XmlReader reader)
    {
        maxDepth = reader.Depth;

        if (HasIdentifier(reader))
        {
            trackedPath.Add(GetPathName(reader));
            unnamedElementCounts.Add(0);
            return;
        }
        trackedPath.Add(reader.NodeType.ToString() + "0");
        unnamedElementCounts.Add(1);
    }

    private void ExtendPath(XNode node, int depth)
    {
        maxDepth = depth; 
        
        if (node.NodeType == XmlNodeType.Element)
        {
            if (HasIdentifierNode(node))
            {
                trackedPath.Add(GetPathNameNode(node));
                unnamedElementCounts.Add(0);
                return; 
            }
        }
        trackedPath.Add(node.NodeType.ToString() + "0");
        unnamedElementCounts.Add(1);
    }


}