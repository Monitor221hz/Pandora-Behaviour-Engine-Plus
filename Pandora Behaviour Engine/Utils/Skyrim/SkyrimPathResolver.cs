// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;

namespace Pandora.Utils;

/// <summary>
/// Resolves the installation path of Skyrim Special Edition, prioritizing a manually specified path via command-line arguments.
/// If not provided, it queries the Windows Registry for the installation path.
/// </summary>
/// <returns>
/// Returns the "Data" directory within the installation folder if found; otherwise, null.
/// </returns>
public static class SkyrimPathResolver
{
	/// <summary>
	/// Resolves the installation path of Skyrim Special Edition by checking command-line arguments first,
	/// then falling back to the Windows Registry if no path is specified.
	/// </summary>
	/// <returns>
	/// A <see cref="DirectoryInfo"/> representing the "Data" directory of the Skyrim Special Edition installation,
	/// or null if the path cannot be resolved.
	/// </returns>
	public static DirectoryInfo? Resolve()
	{
		var argsPath = TryReadSkyrimPathFromCommandLineArgs();
		if (argsPath is not null)
			return argsPath;

		var registryPath = TryReadSkyrimPathFromRegistry();
		if (registryPath is not null)
			return registryPath;

		return null;
	}

	/// <summary>
	/// Attempts to retrieve the Skyrim Special Edition installation path from the Windows Registry.
	/// Queries the "Installed Path" value under the registry key for Skyrim Special Edition.
	/// </summary>
	/// <returns>
	/// A <see cref="DirectoryInfo"/> representing the "Data" directory within the installation folder if found;
	/// otherwise, null if the registry key or value is unavailable or the platform is not Windows.
	/// </returns>
	private static DirectoryInfo? TryReadSkyrimPathFromRegistry()
	{
		if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			return null;

		const string subKey = @"SOFTWARE\Wow6432Node\Bethesda Softworks\Skyrim Special Edition";

		using var key = Registry.LocalMachine.OpenSubKey(subKey, false);
		if (key?.GetValue("Installed Path") is string path)
			return new DirectoryInfo(Path.Combine(path, "Data"));

		return null;
	}

	/// <summary>
	/// Attempts to retrieve the Skyrim Special Edition installation path from command-line arguments
	/// specified in <see cref="LaunchOptions.Current.SkyrimGameDirectory"/>.
	/// </summary>
	/// <returns>
	/// A <see cref="DirectoryInfo"/> representing the "Data" directory within the specified path if provided;
	/// otherwise, null if no path is specified in the command-line arguments.
	/// </returns>
	private static DirectoryInfo? TryReadSkyrimPathFromCommandLineArgs()
	{
		var optionSkyrimGameDir = LaunchOptions.Current?.SkyrimGameDirectory;

		if (optionSkyrimGameDir is not null)
			return new DirectoryInfo(Path.Combine(optionSkyrimGameDir.FullName, "Data"));

		return null;
	}
}