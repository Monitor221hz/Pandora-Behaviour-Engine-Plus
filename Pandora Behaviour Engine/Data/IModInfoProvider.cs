// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Collections.Generic;
using System.Threading.Tasks;
using Pandora.API.Patch;

namespace Pandora.Data;

public interface IModInfoProvider
{
	public Task<List<IModInfo>> GetInstalledMods(string folderPath);
	string SingleRelativePath { get; }
}
