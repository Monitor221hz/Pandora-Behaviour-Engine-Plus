using GameFinder.RegistryUtils;
using GameFinder.StoreHandlers.Steam;
using GameFinder.StoreHandlers.Steam.Models.ValueTypes;
using NexusMods.Paths;
using Pandora.Utils;
using System.IO;

namespace Pandora.Services.CreationEngine.Locators;

public sealed class SteamGameLocator : IGameLocator
{
	private readonly IGameDescriptor _gameDescriptor;
	private readonly IRegistry? _registry;
	private readonly IFileSystem _fileSystem;

	public SteamGameLocator(IGameDescriptor gameDescriptor, IRegistry? registry, IFileSystem fileSystem)
	{
		_gameDescriptor = gameDescriptor;
		_registry = registry;
		_fileSystem = fileSystem;
	}

	public DirectoryInfo? TryLocateGameData()
	{
		if (_registry is null) 
			return null;

		var handler = new SteamHandler(_fileSystem, _registry);

		foreach (var appId in _gameDescriptor.SteamAppIds)
		{
			var game = handler.FindOneGameById(AppId.From(appId), out _);
			if (game?.Path.DirectoryExists() != true)
				continue;

			var gameDir = new DirectoryInfo(game.Path.GetFullPath());

			return GamePathUtils.NormalizeToDataDirectory(gameDir, _gameDescriptor);
		}
		return null;
	}
}

