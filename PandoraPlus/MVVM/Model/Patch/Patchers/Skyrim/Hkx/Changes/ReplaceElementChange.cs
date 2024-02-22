using Pandora.Core;
using System.Xml;
using System.Xml.Linq;

namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public class ReplaceElementChange : IPackFileChange
	{
		public IPackFileChange.ChangeType Type { get;  } = IPackFileChange.ChangeType.Replace;

		public XmlNodeType AssociatedType { get;  } = XmlNodeType.Element;

		public string Path { get; private set; }

		private XElement element { get; set; }



		public ReplaceElementChange(string path, XElement element)
		{
			Path = path;
			this.element = element;
	
		}
		public bool Apply(PackFile packFile)
		{
			if (!packFile.Map.PathExists(Path)) return false;
			element = PackFileEditor.ReplaceElement(packFile, Path, element);
			return element != null; 
		}

		public bool Revert(PackFile packFile)
		{
			return Apply(packFile);
		}
	}





}
