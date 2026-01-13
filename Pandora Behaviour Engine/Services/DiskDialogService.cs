// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Pandora.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Pandora.Services;

public class DiskDialogService : IDiskDialogService
{
	private readonly Window _window;

	public DiskDialogService(Window window)
	{
		_window = window;
	}

	public async Task<DirectoryInfo?> OpenFolderAsync(string title, DirectoryInfo? initialDirectory = null)
	{
		IStorageFolder? startLocation = null;

		if (initialDirectory is { Exists: true })
		{
			startLocation = await _window.StorageProvider
				.TryGetFolderFromPathAsync(initialDirectory.FullName);
		}

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


	public async Task<FileInfo?> OpenFileAsync(string title, params string[] patterns)
	{
		var files = await _window.StorageProvider.OpenFilePickerAsync(
			new FilePickerOpenOptions
			{
				Title = title,
				AllowMultiple = false,
				FileTypeFilter = new List<FilePickerFileType>
				{
					new FilePickerFileType(title) { Patterns = patterns },
				},
			}
		);
		if (files.Count == 0)
		{
			return null;
		}
		var localPath = files[0].TryGetLocalPath();

		return new FileInfo(localPath!);
	}
}
