using Pandora.API.Patch;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Pandora.MVVM.Data;

public interface IModInfoProvider
{
    public Task<List<IModInfo>> GetInstalledMods(string folderPath);
}
