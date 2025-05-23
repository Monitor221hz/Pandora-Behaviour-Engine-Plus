using System.Collections.Generic;
using System.IO;

namespace Pandora.Models.Extensions;

public class FileCache
{
	private Dictionary<string, FileInfo> pathMap = [];

	public FileInfo GetFile(string path)
	{

		if (!pathMap.TryGetValue(path, out FileInfo? fileInfo))
		{
			pathMap.Add(path, fileInfo = new FileInfo(path));
		}

		return fileInfo;
	}

	public FileInfo[] GetFiles(DirectoryInfo directory)
	{
		var fileArray = directory.GetFiles();

		for (int i = 0; i < fileArray.Length; i++)
		{
			if (pathMap.TryGetValue(fileArray[i].FullName, out FileInfo? fileInfo))
			{
				fileArray[i] = fileInfo;
				continue;
			}
			fileInfo = fileArray[i];
			pathMap.Add(fileInfo.FullName, fileInfo);
		}
		return fileArray;
	}
}
