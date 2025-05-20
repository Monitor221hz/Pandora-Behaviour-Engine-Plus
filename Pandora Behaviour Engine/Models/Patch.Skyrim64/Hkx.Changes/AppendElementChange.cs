using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using System.Xml;
using System.Xml.Linq;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Changes;

public class AppendElementChange : IPackFileChange
{
	public IPackFileChange.ChangeType Type { get; } = IPackFileChange.ChangeType.Append;

	public XmlNodeType AssociatedType { get; } = XmlNodeType.Element;

	public string Target { get; }
	public string Path { get; private set; }

	private XElement element { get; set; }
	public AppendElementChange(string target, string path, XElement element)
	{
		Target = target;
		Path = path;
		this.element = element;
	}

	public bool Apply(PackFile packFile)
	{
		if (!packFile.TryGetXMap(Target, out var xmap)) { return false; }
		string newPath = PackFileEditor.AppendElement(xmap!, Path, element);
		Path = string.IsNullOrEmpty(newPath) ? Path : newPath;
		return xmap!.PathExists(Path);
	}
}
