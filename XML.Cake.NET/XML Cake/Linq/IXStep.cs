using System.Xml.Linq;

namespace XmlCake.Linq.Expressions
{
	public interface IXStep
	{
		bool IsMatch(XObject xmlObject);
	}
}