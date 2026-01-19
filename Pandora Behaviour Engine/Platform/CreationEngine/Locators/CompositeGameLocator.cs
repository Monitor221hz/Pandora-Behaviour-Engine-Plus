using Pandora.Paths.Validation;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pandora.Platform.CreationEngine.Locators;

public sealed class CompositeGameLocator : IGameLocator
{
	private readonly IReadOnlyList<IGameLocator> _locators;
	private readonly IGameDataValidator _validator;

	public CompositeGameLocator(
		IEnumerable<IGameLocator> locators,
		IGameDataValidator validator)
	{
		_locators = locators.ToList();
		_validator = validator;
	}

	public DirectoryInfo? TryLocateGameData()
	{
		foreach (var locator in _locators)
		{
			var candidate = locator.TryLocateGameData();
			if (candidate is null)
				continue;

			var normalized = _validator.Normalize(candidate);
			if (normalized is not null)
				return normalized;
		}

		return null;
	}
}
