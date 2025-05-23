using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using System.Xml;
using System.Xml.Linq;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Changes;

public class RemoveElementChange : IPackFileChange
{
	public IPackFileChange.ChangeType Type { get; } = IPackFileChange.ChangeType.Remove;

	public XmlNodeType AssociatedType { get; } = XmlNodeType.Element;

	public string Target { get; }
	public string Path { get; private set; }
	private XElement? element { get; set; }
	public RemoveElementChange(string target, string path)
	{
		Target = target;
		Path = path;
	}
	public bool Apply(PackFile packFile)
	{
		if (!packFile.TryGetXMap(Target, out var xmap))
		{
			return false;
		}
		element = PackFileEditor.RemoveElement(xmap!, Path);
		return !xmap!.PathExists(Path);
	}

}