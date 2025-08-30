// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Microsoft.Win32;
using System.Runtime.Versioning;

namespace Pandora.Utils.Platform.Windows;

[SupportedOSPlatform("windows")]
public sealed class WindowsRegistry : IRegistry
{
	public object? GetValue(RegistryKey root, string subKey, string name)
	{
		using var key = root.OpenSubKey(subKey);
		return key?.GetValue(name);
	}
}
