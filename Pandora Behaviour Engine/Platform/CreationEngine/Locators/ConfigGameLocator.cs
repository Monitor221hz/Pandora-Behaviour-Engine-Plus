using Pandora.Paths.Services;
using System.IO;

namespace Pandora.Platform.CreationEngine.Locators;

public sealed class ConfigGameLocator : IGameLocator
{
	private readonly IPathConfigService _configService;
	private readonly IGameDescriptor _gameDescriptor;

	public ConfigGameLocator(
		IPathConfigService configService,
		IGameDescriptor gameDescriptor)
	{
		_configService = configService;
		_gameDescriptor = gameDescriptor;
	}

	public DirectoryInfo? TryLocateGameData()
	{
		var settings = _configService.GetGamePaths(_gameDescriptor.Id);

		if (settings.GameDataDirectory is { Exists: true } dir)
		{
			return dir;
		}

		return null;
	}
}