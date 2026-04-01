// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Collections.Generic;
using System.IO;

namespace Pandora.Models.Extensions;

public class FileCache
{
	private readonly Dictionary<string, FileInfo> _pathMap = [];

	public FileInfo GetFile(string path)
	{
		if (!_pathMap.TryGetValue(path, out FileInfo? fileInfo))
		{
			_pathMap.Add(path, fileInfo = new FileInfo(path));
		}

		return fileInfo;
	}

	public FileInfo[] GetFiles(DirectoryInfo directory)
	{
		var fileArray = directory.GetFiles();

		for (int i = 0; i < fileArray.Length; i++)
		{
			if (_pathMap.TryGetValue(fileArray[i].FullName, out FileInfo? fileInfo))
			{
				fileArray[i] = fileInfo;
				continue;
			}
			fileInfo = fileArray[i];
			_pathMap.Add(fileInfo.FullName, fileInfo);
		}
		return fileArray;
	}
}