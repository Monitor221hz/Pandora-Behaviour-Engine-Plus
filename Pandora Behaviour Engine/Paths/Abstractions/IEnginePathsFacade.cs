using System.IO;

namespace Pandora.Paths.Abstractions;

public interface IEnginePathsFacade
{
	DirectoryInfo GameDataFolder { get; }
	DirectoryInfo OutputFolder { get; }

	DirectoryInfo AssemblyFolder { get; }
	DirectoryInfo TemplateFolder { get; }
	DirectoryInfo EngineFolder { get; }


	DirectoryInfo OutputEngineFolder { get; }
	DirectoryInfo OutputMeshesFolder { get; }

	FileInfo OutputPreviousFile { get; }
	FileInfo ActiveModsFile { get; }
	FileInfo PathConfig { get; }
}
