using System;
using System.IO;

namespace Pandora.Paths.Contexts;

public sealed class EnginePathContext : IEnginePathContext
{
	private readonly IAppPathContext _appPaths;
	private readonly IUserPathContext _userPaths;
	private readonly IOutputPathContext _outputPaths;

	public EnginePathContext(
		IAppPathContext appPaths,
		IUserPathContext userPaths,
		IOutputPathContext outputPaths)
	{
		_appPaths = appPaths;
		_userPaths = userPaths;
		_outputPaths = outputPaths;
	}

	public DirectoryInfo GameDataFolder => _userPaths.GameData;
	public DirectoryInfo OutputFolder => _userPaths.Output;

	public IObservable<DirectoryInfo> GameDataChanged => _userPaths.GameDataChanged;
	public IObservable<DirectoryInfo> OutputChanged => _userPaths.OutputChanged;

	public void SetGameData(DirectoryInfo dir) => _userPaths.SetGameData(dir);
	public void SetOutput(DirectoryInfo dir) => _userPaths.SetOutput(dir);


	public DirectoryInfo AssemblyFolder => _appPaths.AssemblyDirectory;
	public DirectoryInfo CurrentFolder => _appPaths.CurrentDirectory;
	public DirectoryInfo TemplateFolder => _appPaths.TemplateDirectory;
	public DirectoryInfo EngineFolder => _appPaths.EngineDirectory;

	public FileInfo PathConfig => _appPaths.PathConfig;

	public DirectoryInfo OutputEngineFolder => _outputPaths.PandoraEngineDirectory;
	public DirectoryInfo OutputMeshesFolder => _outputPaths.MeshesDirectory;

	public FileInfo OutputPreviousFile => _outputPaths.PreviousOutputFile;
	public FileInfo ActiveModsFile => _outputPaths.ActiveModsFile;
}
