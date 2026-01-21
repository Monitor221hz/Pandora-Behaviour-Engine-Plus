using GameFinder.RegistryUtils;
using GameFinder.StoreHandlers.GOG;
using NexusMods.Paths;
using System.IO;

namespace Pandora.Platform.CreationEngine.Locators;

public sealed class GogGameLocator(
	IGameDescriptor gameDescriptor,
	IRegistry? registry,
	IFileSystem fileSystem) : IGameLocator
{
	public DirectoryInfo? TryLocateGameData()
	{
		if (registry is null || gameDescriptor.GogAppId is null) 
			return null;

		var handler = new GOGHandler(registry, fileSystem);
		var game = handler.FindOneGameById(GOGGameId.From(gameDescriptor.GogAppId.Value), out _);

		if (game?.Path.DirectoryExists() != true)
			return null;

		var dirInfo = new DirectoryInfo(game.Path.GetFullPath());

		return dirInfo;
	}
}
