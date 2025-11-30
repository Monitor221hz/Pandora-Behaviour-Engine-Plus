using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Pandora.API.Service;

namespace Pandora.Services;

public class DiskDialogService : IDiskDialogService
{
	private readonly Window _window;

	public DiskDialogService(Window window)
	{
		_window = window;
	}

	public async Task<DirectoryInfo?> OpenFolderAsync()
	{
		var folders = await _window.StorageProvider.OpenFolderPickerAsync(
			new FolderPickerOpenOptions
			{
				Title = "Select Folder Containing Game Exe",
				AllowMultiple = false,
			}
		);
		if (folders == null)
		{
			return null;
		}
		var localPath = folders[0].TryGetLocalPath();
		return folders.Count > 0 ? new DirectoryInfo(localPath ?? string.Empty) : null;
	}

	public async Task<FileInfo?> OpenFileAsync()
	{
		var files = await _window.StorageProvider.OpenFilePickerAsync(
			new FilePickerOpenOptions
			{
				Title = "Select Game Executable",
				AllowMultiple = false,
				FileTypeFilter = new List<FilePickerFileType>
				{
					new FilePickerFileType("Executables") { Patterns = new[] { "*.exe" } },
				},
			}
		);
		if (files.Count == 0)
		{
			return null;
		}
		var localPath = files[0].TryGetLocalPath();

		return files.Count > 0 ? new FileInfo(files[0].TryGetLocalPath() ?? string.Empty) : null;
	}
}
