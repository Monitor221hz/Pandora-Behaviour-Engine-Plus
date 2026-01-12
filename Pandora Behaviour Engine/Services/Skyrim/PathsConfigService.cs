using System.IO;
using System.Text.Json;
using Pandora.Utils;

namespace Pandora.Services.Skyrim;

public sealed class PathsConfigService : IPathsConfigService
{
	private const string ENGINE_FOLDER = "Pandora_Engine";
	private const string CONFIG_FILE = "Paths.json";

	private readonly DirectoryInfo _configDir;

	private readonly JsonSerializerOptions _jsonOptions;

	private PathsConfiguration _configuration = new();

	public PathsConfigService(IApplicationPaths appPaths)
	{
		_configDir = new DirectoryInfo(appPaths.AssemblyDirectory.FullName / ENGINE_FOLDER);

		if (!_configDir.Exists)
			_configDir.Create();

		_jsonOptions = new JsonSerializerOptions { WriteIndented = true };
		_jsonOptions.Converters.Add(Converters.DirectoryInfoJsonConverter.Instance);
		_jsonOptions.Converters.Add(Converters.FileInfoJsonConverter.Instance);

		Load();
	}

	public GamePathSettings GetPaths(string gameId)
	{
		if (_configuration.TryGetValue(gameId, out var settings))
		{
			return settings;
		}

		return new GamePathSettings();
	}

	public void UpdatePaths(string gameId, DirectoryInfo? gameData, DirectoryInfo? output)
	{
		if (!_configuration.TryGetValue(gameId, out var settings))
		{
			settings = new GamePathSettings();
			_configuration[gameId] = settings;
		}

		settings.GameDataDirectory = gameData;
		settings.OutputDirectory = output;

		Save();
	}

	public void Load()
	{
		var file = new FileInfo(_configDir.FullName / CONFIG_FILE);
		if (!file.Exists)
			return;

		try
		{
			using var stream = file.OpenRead();

			_configuration = JsonSerializer.Deserialize<PathsConfiguration>(stream, _jsonOptions) ?? new();
		}
		catch (JsonException)
		{
			_configuration = new();
		}
	}

	public void Save()
	{
		var file = new FileInfo(_configDir.FullName / CONFIG_FILE);
		using var stream = file.Create();

		JsonSerializer.Serialize(stream, _configuration, _jsonOptions);
	}
}