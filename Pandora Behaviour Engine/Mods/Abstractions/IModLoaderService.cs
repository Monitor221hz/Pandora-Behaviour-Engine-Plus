// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Pandora.API.Patch;

namespace Pandora.Mods.Abstractions;

public interface IModLoaderService
{
	Task<HashSet<IModInfo>> LoadModsAsync(IEnumerable<DirectoryInfo> directories);
}
