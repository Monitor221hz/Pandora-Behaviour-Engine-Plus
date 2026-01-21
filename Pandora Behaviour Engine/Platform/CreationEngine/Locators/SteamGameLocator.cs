using GameFinder.RegistryUtils;
using GameFinder.StoreHandlers.Steam;
using GameFinder.StoreHandlers.Steam.Models.ValueTypes;
using NexusMods.Paths;
using System.IO;

namespace Pandora.Platform.CreationEngine.Locators;

public sealed class SteamGameLocator(
	IGameDescriptor gameDescriptor,
	IRegistry? registry,
	IFileSystem fileSystem) : IGameLocator
{
	public DirectoryInfo? TryLocateGameData()
	{
		if (registry is null) 
			return null;

		var handler = new SteamHandler(fileSystem, registry);

		foreach (var appId in gameDescriptor.SteamAppIds)
		{
			var game = handler.FindOneGameById(AppId.From(appId), out _);
			if (game?.Path.DirectoryExists() != true)
				continue;

			var gameDir = new DirectoryInfo(game.Path.GetFullPath());

			return gameDir;
		}
		return null;
	}
}

