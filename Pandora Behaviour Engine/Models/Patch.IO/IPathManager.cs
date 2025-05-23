using System.IO;

namespace Pandora.Models.Patch.IO;

public interface IPathManager
{
	public bool Export(FileInfo inFile);

	public DirectoryInfo Import(FileInfo inFile);
}
