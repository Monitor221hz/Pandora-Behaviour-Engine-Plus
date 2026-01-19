using Pandora.Paths.Abstractions;
using Pandora.Services.Settings;
using System;
using System.IO;

namespace Pandora.Paths;

public sealed class EnginePathsFacade : IEnginePathsFacade
{
	private readonly IApplicationPaths _appPaths;
	private readonly IUserPaths _userPaths;
	private readonly IOutputPaths _outputPaths;

	public EnginePathsFacade(
		IApplicationPaths appPaths,
		IUserPaths userPaths,
		IOutputPaths outputPaths)
	{
		_appPaths = appPaths;
		_userPaths = userPaths;
		_outputPaths = outputPaths;
	}

	public DirectoryInfo GameDataFolder => _userPaths.GameData;
	public DirectoryInfo OutputFolder => _userPaths.Output;

	public DirectoryInfo AssemblyFolder => _appPaths.AssemblyDirectory;
	public DirectoryInfo TemplateFolder => _appPaths.TemplateDirectory;
	public DirectoryInfo EngineFolder => _appPaths.EngineDirectory;

	public FileInfo PathConfig => _appPaths.PathConfig;

	public DirectoryInfo OutputEngineFolder => _outputPaths.PandoraEngineDirectory;
	public DirectoryInfo OutputMeshesFolder => _outputPaths.MeshesDirectory;

	public FileInfo OutputPreviousFile => _outputPaths.PreviousOutputFile;
	public FileInfo ActiveModsFile => _outputPaths.ActiveModsFile;
}
