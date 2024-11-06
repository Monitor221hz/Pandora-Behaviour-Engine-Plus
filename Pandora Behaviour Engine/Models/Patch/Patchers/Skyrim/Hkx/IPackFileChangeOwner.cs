using Pandora.API.Patch;
using Pandora.Core;
using System.Xml.Linq;

namespace Pandora.Patch.Patchers.Skyrim.Hkx;
public interface IPackFileChangeOwner
{
	IModInfo Origin { get; set; }
	void AddChange(IPackFileChange change);
	void AddElementAsChange(XElement element);
}