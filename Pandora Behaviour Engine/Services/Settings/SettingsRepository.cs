using Pandora.Paths.Abstractions;
using Pandora.Paths.Configuration.DTOs;
using Pandora.Paths.Contexts;
using System;
using System.IO;
using System.Text.Json;

namespace Pandora.Services.Settings;

public sealed class SettingsRepository(IApplicationPaths appPaths) : ISettingsRepository
{
	private readonly FileInfo _configFile = appPaths.PathConfig;

	public PathsConfiguration Load()
	{
		if (!_configFile.Exists)
			return [];

		try
		{
			using var stream = _configFile.OpenRead();
			if (stream.Length == 0) return [];

			return JsonSerializer.Deserialize(stream, PathsJsonContext.Default.PathsConfiguration)
				   ?? [];
		}
		catch (Exception)
		{
			return [];
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
