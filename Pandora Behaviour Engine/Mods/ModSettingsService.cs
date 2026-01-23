// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using NLog;
using Pandora.DTOs;
using Pandora.Mods.Abstractions;
using Pandora.Paths.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pandora.Mods;

public sealed class ModSettingsService : IModSettingsService
{
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	private readonly IOutputPaths _pathContext;

	public ModSettingsService(IOutputPaths pathContext)
	{
		_pathContext = pathContext;
	}

	public async Task<List<ModSaveEntry>> LoadAsync()
	{
		var path = _pathContext.ActiveModsFile.FullName;
		if (!File.Exists(path)) return [];

		try
		{
			using var stream = File.OpenRead(path);

			var result = await JsonSerializer.DeserializeAsync(
				stream,
				ModsJsonContext.Default.ListModSaveEntry
			);

			return result ?? [];
		}
		catch (Exception ex)
		{
			logger.Warn(ex, "Failed to load mod settings. Returning empty list.");
			return [];
		}
	}

	public async Task SaveAsync(IEnumerable<ModSaveEntry> entries)
	{
		try
		{
			var path = _pathContext.ActiveModsFile.FullName;

			Directory.CreateDirectory(Path.GetDirectoryName(path)!);

			using var stream = File.Create(path);
			await JsonSerializer.SerializeAsync(
				stream,
				[.. entries],
				ModsJsonContext.Default.ListModSaveEntry
			);

			logger.Info("Mod settings saved.");
		}
		catch (Exception ex)
		{
			logger.Error(ex, "Failed to save mod settings.");
			throw;
		}
	}

}
