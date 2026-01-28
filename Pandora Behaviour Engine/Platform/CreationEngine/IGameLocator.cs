// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using System.IO;

namespace Pandora.Platform.CreationEngine;

public interface IGameLocator
{
	DirectoryInfo? TryLocateGameData();
}
