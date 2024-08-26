using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace XmlCake.Linq.Expressions;

public class XWrapExpression : IXExpression
{
	public XWrapExpression(List<IXStep> steps) => matchSteps = steps;
	public XWrapExpression(params IXStep[] steps) => matchSteps = steps.ToList();

	public IXExpression? SkipExpression { get; set; }

	public XMatch Match(List<XNode> nodes)
	{
		int p = 0;
		int skipCount = 0; 
		List<XNode> buffer = new List<XNode>();
		foreach (XNode node in nodes)
		{
			if (skipCount > 0)
			{
				skipCount -= 1;
				continue;
			}
			if (matchSteps[p].IsMatch(node)) { p++; }
			if (p > 0)
			{
				if (node.NodeType == XmlNodeType.Element)
				{
					XElement element = (XElement)node;
					skipCount = element.DescendantNodes().Count(); //skip child nodes 
				}
				buffer.Add(node);

			}

			if (p == matchSteps.Count) 
			{
				if (buffer.Count == matchSteps.Count) break; 
				return new XMatch(buffer); 
			}

		}
		return new XMatch();

	}


	public XMatchCollection Matches(List<XNode> nodes)
	{
		int p = 0;

		int skipCount = 0; 
		List<XMatch> matchList = new List<XMatch>();
		List<XNode> buffer = new List<XNode>();
		foreach (XNode node in nodes)
		{
			if (skipCount > 0)
			{
				skipCount -= 1; 
				continue;
			}
			if (matchSteps[p].IsMatch(node)) { p++;  }
			if (p > 0)
			{
				if (node.NodeType == XmlNodeType.Element)
				{
					XElement element = (XElement)node;
					skipCount = element.DescendantNodes().Count(); //skip child nodes 
				}
				buffer.Add(node);

			}

			if (p == matchSteps.Count)
			{
				if (buffer.Count == matchSteps.Count) break;
				p = 0;
				matchList.Add(new XMatch(new List<XNode>(buffer)));
				buffer.Clear();
			}
			

		}
		return new XMatchCollection(matchList);
	}

	public XMatchCollection Removes(List<XNode> nodes)
	{
		throw new System.NotImplementedException();
	}

	private List<IXStep> matchSteps { get; set; } = new List<IXStep>();
}
