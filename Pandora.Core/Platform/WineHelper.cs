// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Runtime.InteropServices;

namespace Pandora.Core.Platform;

public static class WineHelper
{
	private static readonly Lazy<bool> _isWineRunning = new(CheckWineRunning);

	public static bool IsWineRunning => _isWineRunning.Value;

	private static bool CheckWineRunning()
	{
		if (!OperatingSystem.IsWindows())
			return false;

		try
		{
			var ntdll = GetModuleHandle("ntdll.dll");
			if (ntdll == IntPtr.Zero)
				return false;

			var wineGetVersion = GetProcAddress(ntdll, "wine_get_version");
			return wineGetVersion != IntPtr.Zero;
		}
		catch
		{
			return false;
		}
	}

	[DllImport("kernel32.dll", EntryPoint = "GetModuleHandleW", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern IntPtr GetModuleHandle(string lpModuleName);

	[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
	private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
}
