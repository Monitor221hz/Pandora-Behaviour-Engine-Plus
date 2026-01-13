using GameFinder.RegistryUtils;
using GameFinder.StoreHandlers.GOG;
using NexusMods.Paths;
using System.IO;

namespace Pandora.Services.CreationEngine.Locators;

public sealed class GogGameLocator : IGameLocator
{
	private readonly IGameDescriptor _gameDescriptor;
	private readonly IRegistry? _registry;
	private readonly IFileSystem _fileSystem;

	public GogGameLocator(IGameDescriptor gameDescriptor, IRegistry? registry, IFileSystem fileSystem)
	{
		_gameDescriptor = gameDescriptor;
		_registry = registry;
		_fileSystem = fileSystem;
	}

	public DirectoryInfo? TryLocateGameData()
	{
		if (_registry is null || _gameDescriptor.GogAppId is null) 
			return null;

		var handler = new GOGHandler(_registry, _fileSystem);
		var game = handler.FindOneGameById(GOGGameId.From(_gameDescriptor.GogAppId.Value), out _);

		if (game?.Path.DirectoryExists() != true)
			return null;

		var dirInfo = new DirectoryInfo(game.Path.GetFullPath());

		return dirInfo;
	}
}
