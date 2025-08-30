﻿// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Microsoft.Win32;
using NLog;
using Pandora.Logging;
using Pandora.Utils.Platform.Windows;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Pandora.Utils.Skyrim;

/// <summary>
/// Resolves the installation path of Skyrim Special Edition, prioritizing a manually specified path via command-line arguments.
/// If not provided, it queries the Windows Registry for the installation path.
/// </summary>
/// <returns>
/// Returns the "Data" directory within the installation folder if found; otherwise <see cref="Environment.CurrentDirectory"/>.
/// </returns>
public sealed class SkyrimPathResolver
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public const string RegistrySubKey = @"SOFTWARE\Wow6432Node\Bethesda Softworks\Skyrim Special Edition";
	public const string RegistryValueName = "Installed Path";

	private delegate DirectoryInfo? PathProvider();

	private readonly IRuntimeEnvironment _environment;
	private readonly IRegistry? _registry;
	private readonly DirectoryInfo _currentDirectory;

	public SkyrimPathResolver(IRuntimeEnvironment environment, IRegistry? registry)
	{
		_environment = environment;
		_registry = registry;
		_currentDirectory = new DirectoryInfo(_environment.CurrentDirectory);
	}

	/// <summary>
	/// Resolves the installation path of Skyrim Special Edition by checking command-line arguments first,
	/// then falling back to the Windows Registry if no path is specified.
	/// </summary>
	/// <returns>
	/// A <see cref="DirectoryInfo"/> representing the "Data" directory of the Skyrim Special Edition installation,
	/// otherwise <see cref="Environment.CurrentDirectory"/>.
	/// </returns>
	public DirectoryInfo Resolve()
	{
		PathProvider[] providers =
		[
			TryGetDataPathFromCommandLine,
			TryGetDataPathFromCurrentDirectory,
			() => TryGetDataPathFromRegistry()
		];

		var resolvedPath = providers
			.Select(provider => provider())
			.FirstOrDefault(IsValidSkyrimDataDirectory);

		if (resolvedPath is not null)
		{
			Logger.Info($"Found valid Skyrim 'Data' directory at: {resolvedPath.FullName}");
			return resolvedPath;
		}

		string msg = $"Could not find a valid Skyrim 'Data' directory. Current directory {_environment.CurrentDirectory}";
		EngineLoggerAdapter.AppendLine($"WARN: {msg}");
		Logger.Warn(msg);
		return _currentDirectory;
	}

	/// <summary>
	/// Attempts to retrieve the Skyrim Special Edition installation path from command-line arguments
	/// specified in <see cref="LaunchOptions.Current.SkyrimGameDirectory"/>.
	/// </summary>
	/// <returns>
	/// A <see cref="DirectoryInfo"/> representing the "Data" directory within the specified path if provided;
	/// otherwise, null if no path is specified in the command-line arguments.
	/// </returns>
	private static DirectoryInfo? TryGetDataPathFromCommandLine()
	{
		var gameDir = LaunchOptions.Current?.SkyrimGameDirectory;
		if (gameDir is not null)
		{
			Logger.Debug("Attempting Skyrim path from command line args (--tesv): {0}", gameDir.FullName);
			return NormalizeToDataDirectory(gameDir);
		}
		Logger.Debug("No Skyrim path in command line args.");
		return null;
	}

	/// <summary>
	/// Try to get path from current working directory. 
	/// This is needed in case the path of the current directory coincides with the directory to Data. 
	/// Such cases are set via MO2/Vortex using Start In.
	/// </summary>
	/// <returns>
	/// A <see cref="DirectoryInfo"/> representing the "Data" directory within the installation folder if found; 
	/// otherwise <see cref="Environment.CurrentDirectory"/>.
	/// </returns>
	private DirectoryInfo? TryGetDataPathFromCurrentDirectory()
	{
		Logger.Debug("Attempting Skyrim path from current directory: {0}", _currentDirectory.FullName);
		return NormalizeToDataDirectory(_currentDirectory);
	}

	/// <summary>
	/// Attempts to retrieve the Skyrim Special Edition installation path from the Windows Registry.
	/// Queries the "Installed Path" value under the registry key for Skyrim Special Edition.
	/// </summary>
	/// <returns>
	/// A <see cref="DirectoryInfo"/> representing the "Data" directory within the installation folder if found;
	/// otherwise, null if the registry key or value is unavailable or the platform is not Windows.
	/// </returns>
	public DirectoryInfo? TryGetDataPathFromRegistry(RegistryKey? baseKey = null, string? subKeyOverride = null)
	{
		if (_registry is null || !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			return null;

		try
		{
			var rootKey = baseKey ?? Registry.LocalMachine;
			var finalSubKey = subKeyOverride ?? RegistrySubKey;

			if (_registry.GetValue(rootKey, finalSubKey, RegistryValueName) is string path && !string.IsNullOrEmpty(path))
			{
				Logger.Debug("Found Skyrim path in registry: {0}", path);
				return NormalizeToDataDirectory(new DirectoryInfo(path));
			}

			Logger.Debug("Skyrim registry key/value not found or empty.");
		}
		catch (Exception ex)
		{
			Logger.Warn(ex, "Failed to read Skyrim installation path from registry.");
		}

		return null;
	}

	public static bool IsValidSkyrimDataDirectory([NotNullWhen(true)] DirectoryInfo? dataDirectory)
	{
		if (dataDirectory is null || !dataDirectory.Exists)
			return false;

		if (!dataDirectory.Name.Equals("Data", StringComparison.OrdinalIgnoreCase))
			return false;

		var gameRoot = dataDirectory.Parent;
		if (gameRoot is null || !gameRoot.Exists)
			return false;

		return File.Exists(Path.Combine(gameRoot.FullName, "SkyrimSE.exe")) ||
			   File.Exists(Path.Combine(gameRoot.FullName, "SkyrimSELauncher.exe"));
	}

	private static DirectoryInfo NormalizeToDataDirectory(DirectoryInfo directory)
	{
		if (directory.Name.Equals("Data", StringComparison.OrdinalIgnoreCase))
			return directory;

		return new DirectoryInfo(Path.Combine(directory.FullName, "Data"));
	}
}
