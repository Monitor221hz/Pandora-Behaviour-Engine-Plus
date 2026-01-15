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
			var result = locator.TryLocateGameData();
			if (result is not null)
				return result;
		}

		return null;
	}
}