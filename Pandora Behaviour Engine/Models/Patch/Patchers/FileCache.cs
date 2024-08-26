using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers;
public class FileCache
{
	
	private Dictionary<string, FileInfo> pathMap = new Dictionary<string, FileInfo>();

	public FileInfo GetFile(string path)
	{
		FileInfo? fileInfo = null;

		if (!pathMap.TryGetValue(path, out fileInfo))
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
			FileInfo? fileInfo;
			if (pathMap.TryGetValue(fileArray[i].FullName, out fileInfo))
			{
				fileArray[i] = fileInfo;
				continue;
			}
			fileInfo = fileArray[i];
			pathMap.Add(fileInfo.FullName, fileInfo );
		}
		return fileArray;
	}
}
