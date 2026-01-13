using Pandora.API.Patch;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Pandora.Services.Interfaces;

public interface IModLoaderService
{
    Task<HashSet<IModInfo>> LoadModsAsync(IEnumerable<DirectoryInfo> directories);
}