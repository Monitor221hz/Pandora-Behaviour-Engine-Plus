using Pandora.Paths.Contexts;
using Pandora.Paths.Validation;
using Pandora.Platform.CreationEngine;
using System.IO;

namespace Pandora.Paths.Services;

public sealed class GamePathService : UserPathServiceBase, IGamePathService
{
	private readonly IGameLocator _locator;
	private readonly IGameDataValidator _validator;

	public GamePathService(
		IUserPathContext userPaths,
		IGameDescriptor descriptor,
		IGameLocator locator,
		IGameDataValidator validator,
		IPathConfigService config)
		: base(userPaths, descriptor, config)
	{
		_locator = locator;
		_validator = validator;
	}

	public void Initialize()
	{
		var located = _locator.TryLocateGameData();
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

	public bool NeedsUserSelection => !IsGameDataValid;

	public bool IsGameDataValid =>
		UserPaths.GameData is not null &&
		_validator.IsValid(UserPaths.GameData);

	private void SetGameData(DirectoryInfo dir)
	{
		UserPaths.SetGameData(dir);
		SaveGameData(dir);
	}
}
