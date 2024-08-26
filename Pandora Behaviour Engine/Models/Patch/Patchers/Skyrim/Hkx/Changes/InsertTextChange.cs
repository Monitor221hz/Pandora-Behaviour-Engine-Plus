using Pandora.Core;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Xml;

namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public class InsertTextChange : IPackFileChange
	{
		public IPackFileChange.ChangeType Type { get;  } = IPackFileChange.ChangeType.Insert;

		public XmlNodeType AssociatedType { get; } = XmlNodeType.Text;
		public string Target { get; }
		public string Path { get; private set; }
		private string markerValue;
		private string value;

        public InsertTextChange(string target, string path, string markerValue, string value)
        {
			Target = target;
			Path = path;
			this.markerValue = markerValue;
			this.value = value;
        }
		public bool Apply(PackFile packFile)
		{
			if (!packFile.TryGetXMap(Target, out var xmap))
			{
				return false;
			}
			return PackFileEditor.InsertText(xmap!, Path, markerValue,  value);
		}
	}





}

public class AppendTextChange : IPackFileChange
{
	public IPackFileChange.ChangeType Type { get; } = IPackFileChange.ChangeType.Insert;
	public XmlNodeType AssociatedType { get; } = XmlNodeType.Text;
	public string Target { get; private set; }
	public string Path { get; private set; }
	private string value { get; set; }


	public AppendTextChange(string target, string path, string value)
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
		PackFileEditor.AppendText(xmap!, Path, value);
		return true;
	}


}
