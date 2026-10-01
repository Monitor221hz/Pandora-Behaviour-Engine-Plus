// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

namespace Pandora.API.Patch.Plugins;

/// <summary>
/// UNSAFE - DO NOT USE
/// </summary>
public interface IPluginInfo
{
	public const string FILE_HEADER = "plugin";
	string Name { get; set; }
	string Author { get; set; }
	string Path { get; set; }
}
