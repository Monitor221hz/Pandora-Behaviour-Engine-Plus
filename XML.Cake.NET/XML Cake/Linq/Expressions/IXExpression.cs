
using System.Xml.Linq;

namespace XmlCake.Linq.Expressions;

public interface IXExpression
{
	public XMatchCollection Matches(List<XNode> nodes); 
	public XMatch Match(List<XNode> nodes);

	public XMatchCollection Removes(List<XNode> nodes);

}


