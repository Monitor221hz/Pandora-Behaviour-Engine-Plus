using DynamicData;
using Pandora.API.Patch;
using Pandora.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pandora.Mods.Services;

public interface IModService
{
	IObservable<IChangeSet<ModInfoViewModel>> Connect();

	Task RefreshModsAsync();

	Task SaveSettingsAsync();

	IReadOnlyList<IModInfo> GetActiveMods();
}