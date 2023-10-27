﻿using System.Xml;
using System.Xml.Linq;

namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public class ReplaceElementChange : IPackFileChange
	{
		public IPackFileChange.ChangeType Type { get;  } = IPackFileChange.ChangeType.Replace;

		public XmlNodeType AssociatedType { get;  } = XmlNodeType.Element;

		public string Path { get; private set; }
		//string path, string oldValue, string newValue

		//PackFile packFile, string path, XElement element
		//public XElement oldElement { get; private set; } 
		private XElement element { get; set; }

		public string ModName { get; private set; }

		public ReplaceElementChange(string path, XElement element, string modName)
		{
			Path = path;
			this.element = element;
			ModName = modName;
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