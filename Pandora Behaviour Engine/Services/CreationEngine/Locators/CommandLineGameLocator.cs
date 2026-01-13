using Pandora.DTOs;
using System.IO;

namespace Pandora.Services.CreationEngine.Locators;

public sealed class CommandLineGameLocator : IGameLocator
{
	private readonly IGameDescriptor _gameDescriptor;
	private readonly LaunchOptions _options;

	public CommandLineGameLocator(IGameDescriptor gameDescriptor, LaunchOptions options)
	{
		_gameDescriptor = gameDescriptor;
		_options = options;
	}

	public DirectoryInfo? TryLocateGameData()
	{
		if (_options.SkyrimGameDirectory is null)
			return null;

		return _options.SkyrimGameDirectory;
	}
}
