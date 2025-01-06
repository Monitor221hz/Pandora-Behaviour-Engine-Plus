using Pandora.API.Patch;
using Pandora.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pandora.Core;
//<mod>
//	<name></name>
//	<author></author>
//	<site></site>
//</mod>
[Serializable]
[XmlRoot(ElementName ="mod")]
public class PandoraModInfo : IModInfo
{
	public override int GetHashCode()
	{
		return Code.GetHashCode();
	}
	public bool Equals(IModInfo? other)
	{
		return other == null ? false :
			Code == other.Code &&
			Version == other.Version;
	}

	[XmlIgnore]
	public bool Active { get; set; } = false;

	[XmlIgnore]
	public DirectoryInfo Folder { get; private set; } = new DirectoryInfo("C:");

	[XmlElement(ElementName = "name")]
	public string Name { get;  set; } = "Default";

	[XmlElement(ElementName = "author")]
	public string Author { get; set; } = "Default";

	[XmlElement(ElementName = "site")]
	public string URL { get; set; } = "Default";

	[XmlAttribute(AttributeName = "code")]
	public string Code { get; set; } = "Default";

	[XmlIgnore]
	public Version Version { get; } = new Version(1, 0, 0);

	[XmlIgnore]
	public IModInfo.ModFormat Format { get; } = IModInfo.ModFormat.Pandora;

	[XmlIgnore]
	public uint Priority { get; set; } = 0;

	[XmlIgnore]
	public bool Valid { get; private set; } = true;

	private PandoraModInfo()
	{

	}
	public void FillData(DirectoryInfo folder)
	{
		Folder = folder;
	}

}
