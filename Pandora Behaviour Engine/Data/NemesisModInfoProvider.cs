// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.IO;
using System.Threading.Tasks;
using Pandora.API.Patch;
using Pandora.Models.Patch.Mod;

namespace Pandora.Data;

public class NemesisModInfoProvider : FileBasedModInfoProvider
{
	protected override string InfoFileName => "info.ini";

	public override string SingleRelativePath => Path.Join("Nemesis_Engine", "mod");

	protected override Task<IModInfo?> TryParseAsync(FileInfo infoFile)
	{
		IModInfo? mod = NemesisModInfo.ParseMetadata(infoFile);
		return Task.FromResult(mod);
	}
}
