using Pandora.Core;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Xml;

namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public class RemoveTextChange : IPackFileChange
	{
		public IPackFileChange.ChangeType Type { get; } = IPackFileChange.ChangeType.Remove;

		public XmlNodeType AssociatedType { get; } = XmlNodeType.Text;
		public string Target { get; }	
		public string Path { get; private set; }
		private string value { get; set; }
		public RemoveTextChange(string target,string path, string value)
		{
			Target = target;
			Path = path; 
			this.value = value;
		}
		public bool Apply(PackFile packFile)
		{
			if (!packFile.TryGetXMap(Target, out var xmap))
			{
				return false;
			}
			PackFileEditor.RemoveText(xmap!, Path, value);
			return true;
		}

		public bool Revert(PackFile packFile)
		{
			//PackFileEditor.InsertText(packFile, Path, value);
			return true; 
		}
	}





}
