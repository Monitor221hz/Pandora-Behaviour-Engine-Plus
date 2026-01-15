using Pandora.DTOs;
using System.IO;

namespace Pandora.Paths.Services;

public interface IPathConfigService
{
	GamePathSettings GetGamePaths(string gameId);

	void SaveGameDataPath(string gameId, DirectoryInfo gameData);
	void SaveOutputPath(string gameId, DirectoryInfo output);
}
