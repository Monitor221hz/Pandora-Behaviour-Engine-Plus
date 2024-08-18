using System.Collections.Generic;
using System.Threading.Tasks;
using Pandora.Core;

namespace Pandora.MVVM.Data;

public interface IModInfoProvider
{
    public Task<List<IModInfo>> GetInstalledMods(string folderPath);
}
