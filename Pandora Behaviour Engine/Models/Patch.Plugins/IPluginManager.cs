// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Pandora.API.Patch.Config;
using System.Collections.Generic;
using System.IO;

namespace Pandora.Models.Patch.Plugins;

public interface IPluginManager
{
	IReadOnlyList<IEngineConfigurationPlugin> EngineConfigurationPlugins { get; }
	void LoadAllPlugins(DirectoryInfo assemblyDirectory);
}
