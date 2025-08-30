// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Microsoft.Win32;

namespace Pandora.Utils.Platform.Windows;

public interface IRegistry
{
	object? GetValue(RegistryKey root, string subKey, string name);
}
