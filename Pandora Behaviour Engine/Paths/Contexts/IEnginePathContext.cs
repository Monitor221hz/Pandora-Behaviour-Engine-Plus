using System;
using System.IO;

namespace Pandora.Paths.Contexts;

public interface IEnginePathContext
{
	DirectoryInfo GameDataFolder { get; }
	DirectoryInfo OutputFolder { get; }

	IObservable<DirectoryInfo> GameDataChanged { get; }
	IObservable<DirectoryInfo> OutputChanged { get; }

	void SetGameData(DirectoryInfo dir);
	void SetOutput(DirectoryInfo dir);

	DirectoryInfo AssemblyFolder { get; }
	DirectoryInfo TemplateFolder { get; }
	DirectoryInfo EngineFolder { get; }


	DirectoryInfo OutputEngineFolder { get; }
	DirectoryInfo OutputMeshesFolder { get; }

	FileInfo OutputPreviousFile { get; }
	FileInfo ActiveModsFile { get; }
	FileInfo PathConfig { get; }
}
