// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Reflection;

namespace Pandora.Utils;

public static class AppInfo
{
	public static string Version =>
		Assembly.GetEntryAssembly()?
			.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
			.InformationalVersion.Split('+')[0] ?? "Unknown";
	public static string Name => Assembly.GetExecutingAssembly().GetName().Name ?? "Unknown";
}

