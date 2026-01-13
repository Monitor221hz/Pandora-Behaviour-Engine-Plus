using DynamicData;
using Pandora.ViewModels;
using System;
using System.Threading.Tasks;

namespace Pandora.Services.Interfaces;

public interface IModService
{
	IObservable<IChangeSet<ModInfoViewModel>> Connect();

	Task RefreshModsAsync();

	Task SaveSettingsAsync();
}