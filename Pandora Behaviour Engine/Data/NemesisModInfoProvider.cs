using Pandora.API.Patch;
using Pandora.Models.Patch.Mod;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Pandora.Data;

public class NemesisModInfoProvider : IModInfoProvider
{
    public async Task<List<IModInfo>> GetInstalledMods(string folderPath) => await Task.Run(() => GetInstalledMods(new DirectoryInfo(folderPath)));

    private static List<IModInfo> GetInstalledMods(DirectoryInfo folder)
    {
		List<IModInfo> infoList = [];
        if (!folder.Exists) { return infoList;  }

        var modFolders = folder.GetDirectories();

        foreach( var modFolder in modFolders ) 
        {
            var infoFile = new FileInfo(Path.Join(modFolder.FullName, "info.ini"));
            if (!infoFile.Exists || infoFile.Directory is null) 
            { 
                continue; 
            }
            infoList.Add(NemesisModInfo.ParseMetadata(infoFile));
        }

        return infoList;
	}
}
