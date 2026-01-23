// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Microsoft.Extensions.DependencyInjection;
using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;
using System;

namespace Pandora.Models.Patch.Skyrim64;

public sealed class PatcherFactory : IPatcherFactory, IDisposable
{
	private readonly IServiceScopeFactory _scopeFactory;
	private IServiceScope? _scope;
	private IEngineConfiguration _config;

	public PatcherFactory(
		IServiceScopeFactory scopeFactory,
		IEngineConfiguration config)
	{
		_scopeFactory = scopeFactory;
		_config = config;
	}

	public IPatcher Create()
	{
		_scope?.Dispose();
		_scope = _scopeFactory.CreateScope();

		return (IPatcher)_scope.ServiceProvider.GetRequiredService(_config.PatcherType);
	}

	public void SetConfiguration(IEngineConfiguration config)
	{
		_config = config;
	}

	public void Dispose()
	{
		_scope?.Dispose();
		_scope = null;
	}
}
