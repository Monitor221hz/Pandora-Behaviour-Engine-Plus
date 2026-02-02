// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Paths.Abstractions;
using Pandora.Settings.DTOs;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pandora.Settings;

[JsonSerializable(typeof(RootConfiguration))]
[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class PandoraJsonContext : JsonSerializerContext;

public sealed class SettingsRepository(IApplicationPaths appPaths) : ISettingsRepository
{
	private readonly FileInfo _configFile = appPaths.PathConfig;

	public RootConfiguration Load()
	{
		if (!_configFile.Exists)
			return new RootConfiguration();

		try
		{
			using var stream = _configFile.OpenRead();
			if (stream.Length == 0) return new RootConfiguration();

			return JsonSerializer.Deserialize(stream, PandoraJsonContext.Default.RootConfiguration)
				   ?? new RootConfiguration();
		}
		catch (Exception)
		{
			return new RootConfiguration();
		}
	}

	public void Save(RootConfiguration data)
	{
		if (!_configFile.Directory!.Exists)
			_configFile.Directory.Create();

		using var stream = _configFile.Create();
		JsonSerializer.Serialize(stream, data, PandoraJsonContext.Default.RootConfiguration);
	}
}
