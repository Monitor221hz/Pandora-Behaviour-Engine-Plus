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
    public async Task<List<IModInfo>> GetInstalledMods(string folderPath)
    {
        List<IModInfo> infoList = new List<IModInfo>();
        string[] folders = Directory.GetDirectories(folderPath);
        foreach (string folder in folders)
        {
            infoList.Add(NemesisModInfo.ParseMetadata(new DirectoryInfo(folder)));
        }
        return infoList;
    }
}
