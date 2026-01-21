using Pandora.Paths.Validation;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pandora.Platform.CreationEngine.Locators;

public sealed class CompositeGameLocator(IEnumerable<IGameLocator> locators) : IGameLocator
{
	public DirectoryInfo? TryLocateGameData()
	{
		foreach (var locator in locators)
		{
			var candidate = locator.TryLocateGameData();
			if (candidate is not null)
				return candidate;
		}

		return null;
	}
}
