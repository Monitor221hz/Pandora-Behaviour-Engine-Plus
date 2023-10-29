using System.Xml;

namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public class ReplaceTextChange : IPackFileChange
	{
		public IPackFileChange.ChangeType Type { get; } = IPackFileChange.ChangeType.Replace;

		public XmlNodeType AssociatedType { get; } = XmlNodeType.Text;

		public string Path { get; private set; }
		private string oldValue { get; set; }

		private string newValue { get; set; }

		public string ModName { get; private set; }

		public ReplaceTextChange(string path, string oldvalue, string newvalue, string modName)
		{
			Path = path;
			oldValue = oldvalue;
			newValue = newvalue;
			ModName = modName;
		}
		public bool Apply(PackFile packFile)
		{
			PackFileEditor.ReplaceText(packFile, Path, oldValue, newValue);
			return true; 
		}

		public bool Revert(PackFile packFile)
		{
			PackFileEditor.ReplaceText(packFile, Path, newValue, oldValue);
			return true; 
		}

	}





}
