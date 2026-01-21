using Pandora.DTOs;
using System.IO;

namespace Pandora.Platform.CreationEngine.Locators;

public sealed class CommandLineGameLocator(LaunchOptions options) : IGameLocator
{
	public DirectoryInfo? TryLocateGameData()
	{
		if (options.SkyrimGameDirectory is null)
			return null;

		return options.SkyrimGameDirectory;
	}
}
