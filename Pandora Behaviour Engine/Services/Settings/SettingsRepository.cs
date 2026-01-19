using Pandora.Paths.Abstractions;
using Pandora.Paths.Configuration.DTOs;
using Pandora.Paths.Contexts;
using System;
using System.IO;
using System.Text.Json;

namespace Pandora.Services.Settings;

public sealed class SettingsRepository : ISettingsRepository
{
	private readonly FileInfo _configFile;

	public SettingsRepository(IApplicationPaths appPaths)
	{
		_configFile = appPaths.PathConfig;
	}

	public PathsConfiguration Load()
	{
		if (!_configFile.Exists)
			return new PathsConfiguration();

		try
		{
			using var stream = _configFile.OpenRead();
			if (stream.Length == 0) return new PathsConfiguration();

			return JsonSerializer.Deserialize(stream, PathsJsonContext.Default.PathsConfiguration)
				   ?? new PathsConfiguration();
		}
		catch (Exception)
		{
			return new PathsConfiguration();
		}
	}

	public void Save(PathsConfiguration data)
	{
		if (!_configFile.Directory!.Exists)
			_configFile.Directory.Create();

		using var stream = _configFile.Create();
		JsonSerializer.Serialize(stream, data, PathsJsonContext.Default.PathsConfiguration);
	}
}
