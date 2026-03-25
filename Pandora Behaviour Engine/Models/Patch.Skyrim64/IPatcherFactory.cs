// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;

namespace Pandora.Models.Patch.Skyrim64;

public interface IPatcherFactory
{
	IPatcher Create();
	void SetConfiguration(IEngineConfiguration config);
}