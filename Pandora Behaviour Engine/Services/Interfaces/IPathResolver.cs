using System;
using System.IO;

namespace Pandora.Services.Interfaces;

public interface IPathResolver
{
    IObservable<DirectoryInfo> GameDataFolderChanged { get; }

    DirectoryInfo GetCurrentFolder();
    DirectoryInfo GetGameDataFolder();
    DirectoryInfo GetTemplateFolder();
    DirectoryInfo GetOutputFolder();
    DirectoryInfo GetOutputMeshFolder();
    DirectoryInfo GetPandoraEngineFolder();

    /// <summary>
	/// Gets the real file system path to the folder containing the running executable.
	///
	/// Unlike typical assembly path methods, this returns the actual disk path even when
	/// running inside virtualized environments like Mod Organizer 2 (MO2), bypassing the VFS.
	/// </summary>
    DirectoryInfo GetAssemblyFolder();
    FileInfo GetActiveModsFile();
    FileInfo GetPreviousOutputFile();

    void SetOutputFolder(DirectoryInfo newFolder);
    bool TrySetGameDataFolder(DirectoryInfo newFolder);
}
