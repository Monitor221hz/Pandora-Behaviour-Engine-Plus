using System.IO;

namespace Pandora.Platform.CreationEngine;

public interface IGameLocator
{
	DirectoryInfo? TryLocateGameData();
}
