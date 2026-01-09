// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using static Vanara.PInvoke.NtDll;

namespace Pandora.Utils;

public static class ProcessUtils
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
	private const int InvalidProcessId = -1;

	public static ModManager Source => _parentProcessInfo.Value.Manager;

	private static readonly Lazy<ParentProcessInfo> _parentProcessInfo = new(
		DetectAndCacheParentProcess
	);

	private record ParentProcessInfo(int Pid, ModManager Manager);

	private static readonly List<(string[] keywords, ModManager manager)> _managerDetectors = new()
	{
		(new[] { "modorganizer", "mo2" }, ModManager.ModOrganizer),
		(new[] { "vortex" }, ModManager.Vortex),
	};

	private static ParentProcessInfo DetectAndCacheParentProcess()
	{
		try
		{
			using var currentProcess = Process.GetCurrentProcess();
			using var parentProcess = GetParent(currentProcess);

			if (parentProcess is null)
			{
				Logger.Info("No parent process found or PID was recycled.");
				return new ParentProcessInfo(0, ModManager.None);
			}

			string parentName = parentProcess.ProcessName.ToLowerInvariant();
			var parentPid = parentProcess.Id;

			var detectedManager = DetectManagerFromText(parentName);
			if (detectedManager != ModManager.None)
			{
				Logger.Info($"Detected launcher: {detectedManager}");
				return new ParentProcessInfo(parentPid, detectedManager);
			}

			if (parentName.Contains("proton"))
			{
				var managerInProton = DetectManagerFromCmdline(parentPid);
				if (managerInProton is not ModManager.None)
				{
					Logger.Info($"Detected launcher: {managerInProton} (Proton)");
					return new ParentProcessInfo(parentPid, managerInProton);
				}
				Logger.Info("Proton detected, but no known mod manager found");
			}
			else if (parentName.Contains("wine"))
			{
				var managerInWine = DetectManagerFromCmdline(parentPid);
				if (managerInWine is not ModManager.None)
				{
					Logger.Info($"Detected launcher: {managerInWine} (Wine)");
					return new ParentProcessInfo(parentPid, managerInWine);
				}
				Logger.Info("Wine detected, but no known mod manager found");
			}

			Logger.Info("No known mod manager detected.");
			return new ParentProcessInfo(parentPid, ModManager.None);
		}
		catch (Exception ex)
		{
			Logger.Warn(ex, "Failed to detect parent process.");
		}

		return new ParentProcessInfo(0, ModManager.None);
	}

	private static Process GetParent(Process process)
	{
		try
		{
			int parentPid = GetParentPid(process);
			if (parentPid == InvalidProcessId)
			{
				return null;
			}

			var candidate = Process.GetProcessById(parentPid);

			// If the candidate process was started LATER, the PID was reused.
			if (candidate.StartTime > process.StartTime)
			{
				candidate.Dispose();
				return null;
			}

			return candidate;
		}
		catch (ArgumentException)
		{
			return null;
		}
	}

	private static int GetParentPid(Process process)
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			return GetParentPidWindows(process);

		if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			return GetParentPidLinux(process);

		return InvalidProcessId;
	}

	[SupportedOSPlatform("windows")]
	private static int GetParentPidWindows(Process process)
	{
		try
		{
			PROCESS_BASIC_INFORMATION pbi = NtQueryInformationProcess<PROCESS_BASIC_INFORMATION>(
				process.Handle,
				PROCESSINFOCLASS.ProcessBasicInformation
			);

			return (int)pbi.InheritedFromUniqueProcessId.ToUInt32();
		}
		catch (Exception ex)
		{
			Logger.Warn(ex, "Failed to query process information on Windows.");
			return InvalidProcessId;
		}
	}

	[SupportedOSPlatform("linux")]
	private static int GetParentPidLinux(Process process)
	{
		var statusPath = $"/proc/{process.Id}/status";
		try
		{
			if (File.Exists(statusPath))
			{
				foreach (var line in File.ReadLines(statusPath))
				{
					if (line.StartsWith("PPid:"))
					{
						if (int.TryParse(line.AsSpan(5).Trim(), out int ppid))
							return ppid;
						break;
					}
				}
			}
		}
		catch (Exception ex)
		{
			Logger.Warn(ex, $"Failed to read process status file: {statusPath}");
		}
		return InvalidProcessId;
	}

	[SupportedOSPlatform("linux")]
	private static ModManager DetectManagerFromCmdline(int parentPid)
	{
		try
		{
			string cmdlinePath = $"/proc/{parentPid}/cmdline";
			if (!File.Exists(cmdlinePath))
				return ModManager.None;

			var cmdline = File.ReadAllText(cmdlinePath).Replace('\0', ' ').ToLowerInvariant();

			return DetectManagerFromText(cmdline);
		}
		catch (Exception ex)
		{
			Logger.Warn(ex, $"Failed to inspect cmdline for PID {parentPid}");
		}

		return ModManager.None;
	}

	private static ModManager DetectManagerFromText(string textToSearch)
	{
		foreach (var (keywords, manager) in _managerDetectors)
		{
			if (keywords.Any(keyword => textToSearch.Contains(keyword)))
			{
				return manager;
			}
		}
		return ModManager.None;
	}
}
