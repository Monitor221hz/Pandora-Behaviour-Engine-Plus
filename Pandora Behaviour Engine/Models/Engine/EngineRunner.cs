// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;
using Pandora.Models.Patch.Skyrim64;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandora.Models.Engine;

public sealed class EngineRunner : IEngineRunner
{
	private readonly IPatcherFactory _patcherFactory;
	private IPatcher? _patcher;

	public EngineRunner(IPatcherFactory patcherFactory)
	{
		_patcherFactory = patcherFactory;
	}

	public async Task PreloadAsync()
	{
		_patcher = _patcherFactory.Create();
		await _patcher.PreloadAsync();
	}

	public async Task<EngineResult> RunAsync(IReadOnlyList<IModInfo> mods)
	{
		var timer = Stopwatch.StartNew();

		try
		{
			if (_patcher is null)
				await PreloadAsync();

			_patcher!.SetTarget(mods.ToList());

			if (!await _patcher.UpdateAsync())
			{
				timer.Stop();
				return EngineResult.Fail("Update failed", timer.Elapsed);
			}

			var success = await _patcher.RunAsync();

			timer.Stop();

			return success
				? EngineResult.Success(_patcher.GetPostRunMessages(), timer.Elapsed)
				: EngineResult.Fail(_patcher.GetFailureMessages(), timer.Elapsed);
		}
		catch (Exception ex)
		{
			timer.Stop();
			return EngineResult.Fail($"Critical error: {ex.Message}", timer.Elapsed);
		}
	}

	public Task SwitchConfigurationAsync(IEngineConfiguration config)
	{
		_patcherFactory.SetConfiguration(config);
		_patcher = null;
		return Task.CompletedTask;
	}

}
