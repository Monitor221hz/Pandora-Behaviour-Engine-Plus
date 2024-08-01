using Pandora.Core;
using System;
using System.Xml;
using System.Xml.Linq;

namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public class ReplaceElementChange : IPackFileChange
	{
		public IPackFileChange.ChangeType Type { get;  } = IPackFileChange.ChangeType.Replace;

		public XmlNodeType AssociatedType { get;  } = XmlNodeType.Element;
		public string Target { get; private set; }
		public string Path { get; private set; }
		private XElement element { get; set; }


        public ReplaceElementChange(string target,string path, XElement element)
        {
			Target = target;
			Path = path; 
			this.element = element;
        }
		public bool Apply(PackFile packFile)
		{
			if (!packFile.TryGetXMap(Target, out var xmap))
			{
				return false;
			}
			element = PackFileEditor.ReplaceElement(xmap!, Path, element);
			return element != null; 
		}

		public bool Revert(PackFile packFile)
		{
			return Apply(packFile);
		}
	}





}
