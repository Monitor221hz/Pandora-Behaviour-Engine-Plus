using Pandora.API.Patch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pandora.Data;

public interface IModInfoProvider
{
    public Task<List<IModInfo>> GetInstalledMods(string folderPath);
}
