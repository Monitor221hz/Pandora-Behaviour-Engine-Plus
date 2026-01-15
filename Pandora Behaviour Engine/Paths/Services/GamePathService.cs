using Pandora.Paths.Contexts;
using Pandora.Paths.Validation;
using Pandora.Platform.CreationEngine;
using Pandora.Utils;
using Splat;
using System.IO;

namespace Pandora.Paths.Services;

public sealed class GamePathService : IGamePathService
{
	private readonly IUserPathContext _userPaths;
	private readonly IGameDescriptor _descriptor;
	private readonly IGameLocator _gameLocator;
	private readonly IGameDataValidator _validator;
	private readonly IPathConfigService _configService;

	public GamePathService(
		IUserPathContext userPaths,
		IGameDescriptor descriptor,
		IGameLocator gameLocator,
		IGameDataValidator validator,
		IPathConfigService configService)
	{
		_userPaths = userPaths;
		_validator = validator;
		_descriptor = descriptor;
		_gameLocator = gameLocator;
		_configService = configService;
	}

	public void Initialize()
	{
		var located = _gameLocator.TryLocateGameData();
		if (located is null)
			return;

		var normalized = _validator.Normalize(located);
		if (normalized is null)
			return;

		SetGameData(normalized);
	}

	public bool TrySetGameData(DirectoryInfo input)
	{
		var normalized = _validator.Normalize(input);
		if (normalized is null)
			return false;

		SetGameData(normalized);
		return true;
	}


	public bool IsGameDataValid =>
		_userPaths.GameData is not null &&
		_validator.IsValid(_userPaths.GameData);

	public bool NeedsUserSelection => !IsGameDataValid;

	private void SetGameData(DirectoryInfo dir)
	{
		_userPaths.SetGameData(dir);
		_configService.SaveGameDataPath(_descriptor.Id, dir);
	}
}
