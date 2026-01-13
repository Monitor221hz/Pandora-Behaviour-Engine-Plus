using Pandora.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pandora.Services.Interfaces;

public interface IModSettingsService
{
    Task<List<ModSaveEntry>> LoadAsync();

    Task SaveAsync(IEnumerable<ModSaveEntry> entries);
}
