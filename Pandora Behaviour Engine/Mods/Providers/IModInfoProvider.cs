// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Patch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pandora.Mods.Providers;

public interface IModInfoProvider
{
	Task<List<IModInfo>> GetInstalledMods(string folderPath);
	string SingleRelativePath { get; }
}
