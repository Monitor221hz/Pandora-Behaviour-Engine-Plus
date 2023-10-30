using System;
using System.Xml;
using System.Xml.Linq;

namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public class AppendElementChange : IPackFileChange
	{
		public IPackFileChange.ChangeType Type { get; } = IPackFileChange.ChangeType.Append;

		public XmlNodeType AssociatedType { get; } = XmlNodeType.Element;

		public string Path { get; private set; }

		private XElement element { get; set; }

		public string ModName { get; private set; }

		public AppendElementChange(string path, XElement element, string modName)
		{
			Path = path;
			this.element = element;
			ModName = modName;
		}
		public bool Apply(PackFile packFile)
		{
			if (!packFile.Map.PathExists(Path)) return false;
			string newPath = PackFileEditor.AppendElement(packFile, Path, element);
			Path = String.IsNullOrEmpty(newPath) ? Path : newPath;
			return packFile.Map.PathExists(Path);

		}

		public bool Revert(PackFile packFile)
		{
			if (!packFile.Map.PathExists(Path)) return false;
			PackFileEditor.RemoveElement(packFile, Path);
			return true;
		}
	}




}
