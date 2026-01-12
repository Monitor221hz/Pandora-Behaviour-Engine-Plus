using Pandora.Services.Skyrim;
using Pandora.Utils;
using System.IO;

namespace Pandora.Services.CreationEngine.Locators;

public sealed class ConfigGameLocator : IGameLocator
{
	private readonly IPathsConfigService _configService;
	private readonly IGameDescriptor _gameDescriptor;

	public ConfigGameLocator(
		IPathsConfigService configService,
		IGameDescriptor gameDescriptor)
	{
		_configService = configService;
		_gameDescriptor = gameDescriptor;
	}

	public DirectoryInfo? TryLocateGameData()
	{
		var settings = _configService.GetPaths(_gameDescriptor.Id);

		if (settings.GameDataDirectory is { Exists: true } dir)
		{
			return GamePathUtils.NormalizeToDataDirectory(dir, _gameDescriptor);
		}

		return null;
	}
}