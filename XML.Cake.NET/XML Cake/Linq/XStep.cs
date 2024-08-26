using System.Xml;
using System.Xml.Linq;

namespace XmlCake.Linq.Expressions;

public class XStep : IXStep
{


	public XmlNodeType NodeType { get; private set; }
	public string Name { get; private set; } = string.Empty;
	public string Value { get; private set; } = string.Empty;
	public XStep(XmlNodeType nodeType, string value = "", string name = "")
	{
		NodeType = nodeType;
		Name = name;
		Value = value;
	}
	public XStep(XObject obj) => new XStep(obj.NodeType);

	public int Count { get; private set; }

	public static XStep Parse(string expression)
	{
		throw new NotImplementedException();
	}

	public bool TryCompareValue(XObject xmlObject)
	{
		if (string.IsNullOrEmpty(Value)) return true;
		switch (xmlObject)
		{
			case XElement element:
				return element.Value == Value || element.Value.Contains(Value);
			case XAttribute attribute:
				return attribute.Value == Value || attribute.Value.Contains(Value);
			case XComment comment:
				return comment.Value == Value || comment.Value.Contains(Value);
			default:
				return true;
		}
	}

	public bool TryCompareName(XObject xmlObject)
	{
		if (string.IsNullOrEmpty(Name)) return true;
		switch (xmlObject)
		{
			case XElement element:
				return element.Name == Name;

			case XAttribute attribute:
				return attribute.Name == Name;

			default:
				return true;
		}
	}

	public bool IsMatch(XObject xmlObject)
	{
		if (xmlObject.NodeType != NodeType && xmlObject.NodeType != XmlNodeType.Whitespace) return false;
		return TryCompareValue(xmlObject) && TryCompareName(xmlObject);
	}
}