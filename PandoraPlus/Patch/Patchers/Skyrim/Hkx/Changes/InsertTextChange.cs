using System.Xml;

namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public class InsertTextChange : IPackFileChange
	{
		public IPackFileChange.ChangeType Type { get;  } = IPackFileChange.ChangeType.Insert;

		public XmlNodeType AssociatedType { get; } = XmlNodeType.Text;

		public string Path { get; private set; } 
		private string value { get; set; }	
		public InsertTextChange(string path, string value)
		{
			Path = path; 
			this.value = value;
		}

		public bool Apply(PackFile packFile)
		{
			PackFileEditor.InsertText(packFile, Path, value);
			return true;
		}

		public bool Revert(PackFile packFile)
		{
			PackFileEditor.RemoveText(packFile, Path, value); 
			return true;
		}
	}





}
