using Pandora.Paths.Contexts;
using Pandora.Platform.CreationEngine;
using System.IO;

namespace Pandora.Paths.Services;

public sealed class OutputPathService : IOutputPathService
{
	private readonly IEnginePathContext _paths;
	private readonly IGameDescriptor _descriptor;
	private readonly IPathConfigService _configService;

	public OutputPathService(
		IEnginePathContext paths,
		IGameDescriptor descriptor,
		IPathConfigService configService)
	{
		_paths = paths;
		_descriptor = descriptor;
		_configService = configService;
	}

	public void SetOutputFolder(DirectoryInfo folder)
	{
		if (!folder.Exists)
			folder.Create();

		_paths.SetOutput(folder);
		_configService.SaveOutputPath(_descriptor.Id, folder);
	}

	public void Initialize()
	{
		var savedPaths = _configService.GetGamePaths(_descriptor.Id);

		if (savedPaths.OutputDirectory is { Exists: true } savedOutput)
		{
			_paths.SetOutput(savedOutput);
		}
		else
		{
			_paths.SetOutput(_paths.GameDataFolder);
		}
	}
}
