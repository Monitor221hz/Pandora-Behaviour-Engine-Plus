using Pandora.DTOs;
using Pandora.Paths.Contexts;
using System;
using System.IO;
using System.Text.Json;

namespace Pandora.Paths.Services;

public sealed class PathConfigService : IPathConfigService
{
	private readonly FileInfo _configFile;
	private readonly JsonSerializerOptions _jsonOptions;

	private PathsConfiguration _configuration = new();

	public PathConfigService(IAppPathContext appPaths)
	{
		_configFile = appPaths.PathConfig;

		if (!_configFile.Directory!.Exists)
			_configFile.Directory.Create();

		_jsonOptions = new JsonSerializerOptions { WriteIndented = true };
		_jsonOptions.Converters.Add(Converters.DirectoryInfoJsonConverter.Instance);
		_jsonOptions.Converters.Add(Converters.FileInfoJsonConverter.Instance);

		Load();
	}

	public GamePathSettings GetGamePaths(string gameId)
	{
		return _configuration.TryGetValue(gameId, out var settings)
			? settings
			: new GamePathSettings();
	}

	public void SaveGameDataPath(string gameId, DirectoryInfo gameData)
	{
		var settings = GetOrCreate(gameId);
		settings.GameDataDirectory = gameData;
		Save();
	}

	public void SaveOutputPath(string gameId, DirectoryInfo output)
	{
		var settings = GetOrCreate(gameId);
		settings.OutputDirectory = output;
		Save();
	}

	private GamePathSettings GetOrCreate(string gameId)
	{
		if (!_configuration.TryGetValue(gameId, out var settings))
		{
			settings = new GamePathSettings();
			_configuration[gameId] = settings;
		}
		return settings;
	}

	private void Load()
	{
		if (!_configFile.Exists) return;

		try
		{
			using var stream = _configFile.OpenRead();
			if (stream.Length == 0) return;

			_configuration = JsonSerializer.Deserialize<PathsConfiguration>(stream, _jsonOptions) ?? new();
		}
		catch (Exception ex)
		{
			_configuration = new();
		}
	}

	private void Save()
	{
		using var stream = _configFile.Create();
		JsonSerializer.Serialize(stream, _configuration, _jsonOptions);
	}
}
