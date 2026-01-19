using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pandora.Services.Settings;

public interface ISettingsService
{
	bool IsGameDataValid { get; }
	bool NeedsUserSelection { get; }

	void Initialize();
	void SetGameDataFolder(DirectoryInfo dir);
	void SetOutputFolder(DirectoryInfo dir);
}