// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;

namespace Pandora.Core.Patch;

public interface IPatcherFactory
{
	IPatcher Create();
	void SetConfiguration(IEngineConfiguration config);
}