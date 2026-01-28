// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿namespace Pandora.Platform.CreationEngine;

public interface IGameDescriptor
{
	string Id { get; }
	string Name { get; }
	uint[] SteamAppIds { get; }
	long? GogAppId { get; }
	string SubKey { get; }
	string[] ExecutableNames { get; }
}
