using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace Pandora.Services;

public class StoragePickerService
{
	private readonly Window _window;

	public StoragePickerService(Window window)
	{
		_window = window;
	}

	public async Task<IStorageFolder?> OpenFolderAsync()
	{
		var folders = await _window.StorageProvider.OpenFolderPickerAsync(
			new FolderPickerOpenOptions
			{
				Title = "Select Folder Containing Game Exe",
				AllowMultiple = false,
			}
		);
		return folders.Count > 0 ? folders[0] : null;
	}
}
