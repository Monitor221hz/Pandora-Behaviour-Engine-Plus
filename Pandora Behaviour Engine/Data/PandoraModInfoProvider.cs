using Pandora.API.Patch;
using Pandora.Models.Patch.Mod;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Pandora.Data;

public class PandoraModInfoProvider : FileBasedModInfoProvider
{
	protected override string InfoFileName => "info.xml";

	public override string SingleRelativePath => Path.Join("Pandora_Engine", "mod");

	protected override async Task<IModInfo?> TryParseAsync(FileInfo infoFile)
	{
		try
		{
			await using var stream = infoFile.OpenRead();
			using var reader = XmlReader.Create(stream);
			if (serializer.Deserialize(reader) is PandoraModInfo modInfo)
			{
				modInfo.FillData(infoFile.Directory!);
				return modInfo;
			}
		}
		catch (Exception ex)
		{
			Debug.Write($"Error: {ex}");
		}
		return null;
	}


	private static readonly XmlSerializer serializer = new(typeof(PandoraModInfo));
}
