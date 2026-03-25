// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Collections.Generic;

namespace Pandora.Settings.DTOs;

public class RootConfiguration
{
	public AppSettings App { get; set; } = new();
	public Dictionary<string, GameSettings> Games { get; set; } = [];
}