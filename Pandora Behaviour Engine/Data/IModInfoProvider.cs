using Pandora.API.Patch;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Pandora.Data;

public interface IModInfoProvider
{
    public Task<List<IModInfo>> GetInstalledMods(string folderPath);
	string SingleRelativePath { get; }
}
