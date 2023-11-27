using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandora.Core;
using Pandora.MVVM.Model;
namespace Pandora.MVVM.Data;



public interface IModInfoProvider
{
    public Task<List<IModInfo>> GetInstalledMods(string folderPath);
}

public class NemesisModInfoProvider : IModInfoProvider
{
    public async Task<List<IModInfo>> GetInstalledMods(string folderPath) => await Task.Run(() => GetInstalledMods(new DirectoryInfo(folderPath)));

    public List<IModInfo> GetInstalledMods(DirectoryInfo folder)
    {
		List<IModInfo> infoList = new List<IModInfo>();
        if (!folder.Exists) { return infoList;  }

        List<FileInfo> infoFiles = new List<FileInfo>();
        var modFolders = folder.GetDirectories();

        foreach( var modFolder in modFolders ) 
        {       
            var files = modFolder.GetFiles("*.ini");
            if (files.Length == 0) { continue; }

            foreach( var file in files ) { infoFiles.Add(file); }
        }

        foreach (var file in infoFiles)
        {
            if (file.Directory == null) continue; 

            infoList.Add(NemesisModInfo.ParseMetadata(file));
        }
        return infoList;
	}
}
