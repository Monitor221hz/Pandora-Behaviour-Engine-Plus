using System;
using System.IO;

namespace Pandora.Paths.Services;

public interface IOutputPathService
{
	void SetOutputFolder(DirectoryInfo folder);

	void Initialize();
}
