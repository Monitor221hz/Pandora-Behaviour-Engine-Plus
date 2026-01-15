using System.IO;

namespace Pandora.Paths.Contexts;

public interface IOutputPathContext
{
	DirectoryInfo PandoraEngineDirectory { get; }
	DirectoryInfo MeshesDirectory { get; }

	FileInfo ActiveModsFile { get; }
	FileInfo PreviousOutputFile { get; }
}
