using Pandora.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pandora.Mods.Abstractions;

public interface IModSettingsService
{
    Task<List<ModSaveEntry>> LoadAsync();

    Task SaveAsync(IEnumerable<ModSaveEntry> entries);
}
