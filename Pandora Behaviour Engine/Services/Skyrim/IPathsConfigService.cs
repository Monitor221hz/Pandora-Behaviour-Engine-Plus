using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pandora.Services.Skyrim;

public interface IPathsConfigService
{
	GamePathSettings GetPaths(string gameId);

	void UpdatePaths(string gameId, DirectoryInfo? gameData, DirectoryInfo? output);

	void Load();
	void Save();
}