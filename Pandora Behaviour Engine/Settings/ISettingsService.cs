using System.IO;

namespace Pandora.Settings;

public interface ISettingsService
{
	bool IsGameDataValid { get; }
	bool NeedsUserSelection { get; }

	void Initialize();
	void SetGameDataFolder(DirectoryInfo dir);
	void SetOutputFolder(DirectoryInfo dir);
}