using Pandora.API.Patch;
using System.Xml.Linq;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Changes;

public interface IPackFileChangeOwner
{
	IModInfo Origin { get; set; }
	void AddChange(IPackFileChange change);
	void AddElementAsChange(XElement element);
}