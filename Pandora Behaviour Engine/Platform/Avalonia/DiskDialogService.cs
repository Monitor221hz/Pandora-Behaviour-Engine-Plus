// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.IO;
using System.Threading.Tasks;

namespace Pandora.Platform.Avalonia;

public sealed class DiskDialogService(Window window) : IDiskDialogService
{
	private readonly Window _window = window;

	public async Task<DirectoryInfo?> OpenFolderAsync(string title, DirectoryInfo? initialDirectory = null)
	{
		var startLocation = await GetStartLocationAsync(initialDirectory);

		var folders = await _window.StorageProvider.OpenFolderPickerAsync(
			new FolderPickerOpenOptions
			{
				Title = title,
				AllowMultiple = false,
				SuggestedStartLocation = startLocation
			}
		);

		if (folders.Count == 0)
			return null;

		var localPath = folders[0].TryGetLocalPath();
		return localPath is null ? null : new DirectoryInfo(localPath);
	}


	public async Task<FileInfo?> OpenFileAsync(string title, DirectoryInfo? initialDirectory = null, params string[] patterns)
	{
		var startLocation = await GetStartLocationAsync(initialDirectory);

		var files = await _window.StorageProvider.OpenFilePickerAsync(
			new FilePickerOpenOptions
			{
				Title = title,
				AllowMultiple = false,
				FileTypeFilter =
				[
					new(title) { Patterns = patterns },
				],
				SuggestedStartLocation = startLocation
			}
		);

		if (files.Count == 0)
			return null;

		var localPath = files[0].TryGetLocalPath();
		return new FileInfo(localPath!);
	}



	private async Task<IStorageFolder?> GetStartLocationAsync(DirectoryInfo? initialDirectory)
	{
		if (initialDirectory?.Exists != true)
		{
			return null;
		}

		return await _window.StorageProvider.TryGetFolderFromPathAsync(initialDirectory.FullName);
	}
}
