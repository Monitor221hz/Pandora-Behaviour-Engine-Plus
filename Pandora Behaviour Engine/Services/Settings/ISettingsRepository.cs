using Pandora.Paths.Configuration.DTOs;
using System.IO;

namespace Pandora.Services.Settings;

public interface ISettingsRepository
{
	PathsConfiguration Load();
	void Save(PathsConfiguration data);
}
