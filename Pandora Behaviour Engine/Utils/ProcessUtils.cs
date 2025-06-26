using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Pandora.Utils;

public static class ProcessUtils
{
	public static bool IsLaunchedFromModOrganizer()
	{
		try
		{
			using var process = Process.GetCurrentProcess();
			int parentPid = GetParentProcessId(process.Id);
			if (parentPid == 0) return false;

			using var parent = Process.GetProcessById(parentPid);
			var parentName = parent.ProcessName.ToLowerInvariant();

			if (parentName.Contains("modorganizer") || parentName.Contains("mo2"))
				return true;

			if (parentName.Contains("wine"))
				return TryDetectModOrganizerViaWine(parentPid);

			return false;
		}
		catch
		{
			return false;
		}
	}

	private static int GetParentProcessId(int pid)
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			using var searcher = new System.Management.ManagementObjectSearcher(
				$"SELECT ParentProcessId FROM Win32_Process WHERE ProcessId = {pid}");

			foreach (var obj in searcher.Get())
			{
				return Convert.ToInt32(obj["ParentProcessId"]);
			}
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
			var statusPath = $"/proc/{pid}/status";
			if (File.Exists(statusPath))
			{
				var lines = File.ReadAllLines(statusPath);
				var ppidLine = lines.FirstOrDefault(l => l.StartsWith("PPid:"));
				if (ppidLine != null && int.TryParse(ppidLine.Split(':')[1].Trim(), out int ppid))
				{
					return ppid;
				}
			}
		}

		return 0;
	}

	private static bool TryDetectModOrganizerViaWine(int winePid)
	{
		try
		{
			string cmdlinePath = $"/proc/{winePid}/cmdline";
			if (File.Exists(cmdlinePath))
			{
				var cmdline = File.ReadAllText(cmdlinePath).ToLowerInvariant();
				return cmdline.Contains("modorganizer") || cmdline.Contains("mo2");
			}
		}
		catch { }

		return false;
	}
}
