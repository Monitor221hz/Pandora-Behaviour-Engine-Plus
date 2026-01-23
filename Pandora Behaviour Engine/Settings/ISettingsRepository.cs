using Pandora.Paths.Configuration.DTOs;
using System.IO;

namespace Pandora.Settings;

public interface ISettingsRepository
{
	PathsConfiguration Load();
	void Save(PathsConfiguration data);
}
