using System.IO;

namespace Pandora.Paths.Abstractions;

public interface IOutputPaths
{
	DirectoryInfo PandoraEngineDirectory { get; }
	DirectoryInfo MeshesDirectory { get; }

	FileInfo ActiveModsFile { get; }
	FileInfo PreviousOutputFile { get; }
}
