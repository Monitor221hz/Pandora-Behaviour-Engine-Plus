using Pandora.Paths.Validation;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pandora.Platform.CreationEngine.Locators;

public sealed class CompositeGameLocator : IGameLocator
{
	private readonly IReadOnlyList<IGameLocator> _locators;

	public CompositeGameLocator(IEnumerable<IGameLocator> locators)
	{
		_locators = locators.ToList();
	}

	public DirectoryInfo? TryLocateGameData()
	{
		foreach (var locator in _locators)
		{
			var candidate = locator.TryLocateGameData();
			if (candidate is not null)
				return candidate;
		}

		return null;
	}
}
