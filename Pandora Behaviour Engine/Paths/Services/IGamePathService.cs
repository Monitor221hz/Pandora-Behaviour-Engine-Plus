using System;
using System.IO;

namespace Pandora.Paths.Services;

public interface IGamePathService
{
	bool IsGameDataValid { get; }
	bool NeedsUserSelection { get; }
	bool TrySetGameData(DirectoryInfo input);

	void Initialize();

}