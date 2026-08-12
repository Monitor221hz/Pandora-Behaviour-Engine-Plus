// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

namespace Pandora.API.Patch.Plugins;

public class PluginInfo : IPluginInfo
{
	public string? Name { get; set; }
	public string? Author { get; set; }
	public string? Path { get; set; }
}
