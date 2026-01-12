using Pandora.Services.CreationEngine;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Pandora.Utils;

internal static class GamePathUtils
{
	public static DirectoryInfo? NormalizeToDataDirectory(
		DirectoryInfo input,
		IGameDescriptor descriptor)
	{
		if (!input.Exists)
			return null;

		DirectoryInfo dataDir = input.Name.Equals("Data", StringComparison.OrdinalIgnoreCase)
			? input
			: new DirectoryInfo(Path.Combine(input.FullName, "Data"));

		return IsValidDataDirectory(dataDir, descriptor)
			? dataDir
			: null;
	}

	private static bool IsValidDataDirectory(
		[NotNullWhen(true)] DirectoryInfo? dataDir,
		IGameDescriptor descriptor)
	{
		if (dataDir is null || !dataDir.Exists)
			return false;

		if (!dataDir.Name.Equals("Data", StringComparison.OrdinalIgnoreCase))
			return false;

		var gameRoot = dataDir.Parent;
		if (gameRoot is null || !gameRoot.Exists)
			return false;

		foreach (var exeName in descriptor.ExecutableNames)
		{
			if (File.Exists(Path.Combine(gameRoot.FullName, exeName)))
				return true;
		}

		return false;
	}
}
