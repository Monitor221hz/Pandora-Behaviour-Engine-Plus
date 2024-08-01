using Pandora.Core;
using System;
using System.Xml;

namespace Pandora.Patch.Patchers.Skyrim.Hkx;

public class ReplaceTextChange : IPackFileChange
{
	public IPackFileChange.ChangeType Type { get; } = IPackFileChange.ChangeType.Replace;

	public XmlNodeType AssociatedType { get; } = XmlNodeType.Text;

	public string Target { get; }
	public string Path { get; private set; }

	private string oldValue;

	private string newValue;

	private string preValue;


    public ReplaceTextChange(string target,string path, string preValue, string oldvalue, string newvalue)
    {
		Target = target;
		Path = path;
		oldValue = oldvalue;
		newValue = newvalue;
		this.preValue = preValue;
	}
	public bool Apply(PackFile packFile)
	{
		if (!packFile.TryGetXMap(Target, out var xmap))
		{
			return false;
		}
		return PackFileEditor.ReplaceText(xmap!, Path, preValue, oldValue, newValue);
	}

	public bool Revert(PackFile packFile)
	{
		//PackFileEditor.ReplaceText(packFile, Path, newValue, oldValue);
		return true; 
	}

}
