using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using System.Xml;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Changes;

public interface IPackFileChange
{
	public enum ChangeType
	{
		Remove,
		Insert,
		Replace,
		Append,


	}
	public bool Apply(PackFile packFile);
	public ChangeType Type { get; }
	public XmlNodeType AssociatedType { get; }
	public string Path { get; }
	public string Target { get; }
}