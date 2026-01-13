using Pandora.DTOs;
using Pandora.Services.Interfaces;
using System.IO;

namespace Pandora.Services.Skyrim;

public sealed class OutputDirectoryProvider : IOutputDirectoryProvider
{
	private readonly LaunchOptions _launchOptions;
	private readonly IPathsConfigService _configService;

	public OutputDirectoryProvider(
		LaunchOptions launchOptions,
		IPathsConfigService configService)
	{
		_launchOptions = launchOptions;
		_configService = configService;
	}

	public DirectoryInfo? TryGetOutputDirectory(string gameId)
	{
		if (_launchOptions.OutputDirectory is { Exists: true } cliDir)
			return cliDir;

		var settings = _configService.GetPaths(gameId);

		if (settings.OutputDirectory is { Exists: true } savedDir)
			return savedDir;

		return null;
	}
}