using Pandora.API.Patch;
using Pandora.Models.Patch.Mod;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Pandora.Data;

public class PandoraModInfoProvider : IModInfoProvider
{
	public async Task<List<IModInfo>> GetInstalledMods(string folderPath) => await Task.Run(() => GetInstalledMods(new DirectoryInfo(folderPath)));

	private static readonly XmlSerializer xmlSerializer = new(typeof(PandoraModInfo));
	private static List<IModInfo> GetInstalledMods(DirectoryInfo folder)
	{
		List<IModInfo> infoList = [];
		if (!folder.Exists) { return infoList; }

		var modFolders = folder.GetDirectories();

		foreach (var modFolder in modFolders)
		{
			//var files = modFolder.GetFiles("info.xml");
			var infoFile = new FileInfo(Path.Join(modFolder.FullName, "info.xml"));
			if (!infoFile.Exists || infoFile.Directory is null) { continue; }
			using (var readStream = infoFile.OpenRead())
			{
				using (var xmlReader = XmlReader.Create(readStream))
				{
					var modInfoObj = xmlSerializer.Deserialize(xmlReader);
					if (modInfoObj == null) { continue; }

					var modInfo = (PandoraModInfo)modInfoObj;
					modInfo.FillData(infoFile.Directory);
					infoList.Add(modInfo);
				}
			}
		}
		return infoList;
	}
}
