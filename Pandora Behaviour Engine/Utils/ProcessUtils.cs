using Pandora.API.ModManager;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Pandora.Utils;

public static class ProcessUtils
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

	public static ModManager GetModManagerSource()
	{
		try
		{
			using var process = Process.GetCurrentProcess();
			int parentPid = GetParentProcessId(process.Id);
			if (parentPid == 0) return ModManager.None;

			using var parent = Process.GetProcessById(parentPid);
			string parentName = parent.ProcessName.ToLowerInvariant();

			if (parentName.Contains("modorganizer") || parentName.Contains("mo2"))
			{
				Logger.Info("Detected launcher: Mod Organizer (MO2)");
				return ModManager.ModOrganizer;
			}

			if (parentName.Contains("vortex"))
			{
				Logger.Info("Detected launcher: Vortex");
				return ModManager.Vortex;
			}

			if (parentName.Contains("wine"))
			{
				if (TryDetectModOrganizerViaWine(parentPid))
				{
					Logger.Info("Detected launcher: Mod Organizer (Wine)");
					return ModManager.ModOrganizer;
				}

				if (TryDetectVortexViaWine(parentPid))
				{
					Logger.Info("Detected launcher: Vortex (Wine)");
					return ModManager.Vortex;
				}

				Logger.Info("Wine detected, but no known mod manager found");
			}

			Logger.Info("No known mod manager detected.");
		}
		catch (Exception ex)
		{
			Logger.Warn(ex, "Failed to detect parent process.");
		}

		return ModManager.None;
	}


	private static int GetParentProcessId(int pid)
	{
		try
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				using var searcher = new System.Management.ManagementObjectSearcher(
					$"SELECT ParentProcessId FROM Win32_Process WHERE ProcessId = {pid}");

				foreach (var obj in searcher.Get())
					return Convert.ToInt32(obj["ParentProcessId"]);
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				var statusPath = $"/proc/{pid}/status";
				if (File.Exists(statusPath))
				{
					var lines = File.ReadAllLines(statusPath);
					var ppidLine = lines.FirstOrDefault(l => l.StartsWith("PPid:"));
					if (ppidLine != null && int.TryParse(ppidLine.Split(':')[1].Trim(), out int ppid))
						return ppid;
				}
			}
		}
		catch (Exception ex)
		{
			Logger.Warn(ex, $"Failed to get parent PID for PID {pid}");
		}

		return 0;
	}

	private static bool TryDetectModOrganizerViaWine(int winePid) =>
		CheckCmdlineForPatterns(winePid, ["modorganizer", "mo2"]);

	private static bool TryDetectVortexViaWine(int winePid) =>
		CheckCmdlineForPatterns(winePid, ["vortex"]);

	private static bool CheckCmdlineForPatterns(int pid, string[] patterns)
	{
		try
		{
			string cmdlinePath = $"/proc/{pid}/cmdline";
			if (!File.Exists(cmdlinePath)) return false;

			var cmdline = File.ReadAllText(cmdlinePath).ToLowerInvariant();
			return patterns.Any(p => cmdline.Contains(p));
		}
		catch (Exception ex)
		{
			Logger.Warn(ex, $"Failed to inspect cmdline for PID {pid}");
			return false;
		}
	}
}
