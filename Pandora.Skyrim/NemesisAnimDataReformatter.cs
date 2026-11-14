// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Text;
using Pandora.API.Patch;
using Pandora.Core.Patch.Mod;
using Pandora.Models.Patch.Skyrim64.Format.Nemesis;
using static Pandora.API.Patch.IPatcher;

namespace Pandora.Skyrim;

public class NemesisAnimDataReformatter : IPatcher
{
	private readonly List<NemesisModInfo> _mods = [];
	private readonly StringBuilder _messages = new();
	private readonly StringBuilder _failures = new();

	public PatcherFlags Flags { get; private set; } = PatcherFlags.None;

	public string GetFailureMessages() => _failures.ToString();

	public string GetPostRunMessages() => _messages.ToString();

	public Version GetVersion() => new(1, 0, 0);

	public string GetVersionString() => GetVersion().ToString();

	public Task PreloadAsync() => Task.CompletedTask;

	public async Task<bool> RunAsync()
	{
		if (_mods.Count == 0)
		{
			_messages.AppendLine("No Nemesis mods were selected.");
			Flags = PatcherFlags.Success;
			return true;
		}

		int files = 0;
		int mods = 0;
		foreach (NemesisModInfo mod in _mods)
		{
			try
			{
				int changed = await Task.Run(() =>
					NemesisSetConditionFormatter.ReformatMod(mod.Folder)
				);
				if (changed == 0)
				{
					continue;
				}

				files += changed;
				mods++;
				_messages.AppendLine($"Reformatted {changed} animation set file(s) in {mod.Name}.");
			}
			catch (Exception ex)
			{
				_failures.AppendLine(
					$"Reformat Nemesis Animdata > {mod.Name} > FAILED > {ex.Message}"
				);
			}
		}

		if (_failures.Length > 0)
		{
			Flags = PatcherFlags.LaunchFailed;
			return false;
		}

		if (files == 0)
		{
			_messages.AppendLine("No Nemesis animation set condition edits needed reformatting.");
		}
		else
		{
			_messages.AppendLine(
				$"Reformatted {files} animation set file(s) across {mods} Nemesis mod(s)."
			);
		}

		Flags = PatcherFlags.Success;
		return true;
	}

	public void SetTarget(List<IModInfo> mods)
	{
		_mods.Clear();
		foreach (IModInfo mod in mods)
		{
			if (mod is NemesisModInfo nemesisMod)
			{
				_mods.Add(nemesisMod);
			}
		}
	}

	public Task<bool> UpdateAsync() => Task.FromResult(true);
}
