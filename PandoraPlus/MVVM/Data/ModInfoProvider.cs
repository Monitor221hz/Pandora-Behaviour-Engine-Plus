using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandora.MVVM.Model;
namespace Pandora.MVVM.Data
{
    public interface IModInfoProvider
    {
        public Task<List<ModInfo>> GetInstalledMods(string folderPath);
    }

    public class ModInfoProvider : IModInfoProvider
    {
        public async Task<List<ModInfo>> GetInstalledMods(string folderPath)
        {
            List<ModInfo> infoList = new List<ModInfo>();
            string[] folders = Directory.GetDirectories(folderPath);
            foreach (string folder in folders)
            {
                infoList.Add(new ModInfo(folder));
            }
            return infoList;
        }
    }
}
