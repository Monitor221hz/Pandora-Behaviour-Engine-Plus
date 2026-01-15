using Pandora.Platform.CreationEngine;
using System;
using System.IO;
using Pandora.Paths.Extensions;

namespace Pandora.Paths.Validation;

public sealed class GameDataValidator : IGameDataValidator
{
	private readonly IGameDescriptor _descriptor;

	public GameDataValidator(IGameDescriptor descriptor)
	{
		_descriptor = descriptor;
	}

	public DirectoryInfo? Normalize(DirectoryInfo input)
	{
		if (!input.Exists)
			return null;

		var dataDir = input.Name.Equals("Data", StringComparison.OrdinalIgnoreCase)
			? input
			: new DirectoryInfo(input.FullName / "Data");

		return IsValid(dataDir) ? dataDir : null;
	}

	public bool IsValid(DirectoryInfo input)
	{
		if (!input.Exists || !input.Name.Equals("Data", StringComparison.OrdinalIgnoreCase))
			return false;

		var gameRoot = input.Parent;
		if (gameRoot is null || !gameRoot.Exists)
			return false;

		foreach (var exe in _descriptor.ExecutableNames)
		{
			if (File.Exists(gameRoot.FullName / exe))
				return true;
		}

		return false;
	}
}
