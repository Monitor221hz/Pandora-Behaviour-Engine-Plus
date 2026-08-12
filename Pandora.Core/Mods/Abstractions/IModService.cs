// SPDX-License-Identifier-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.API.Patch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pandora.Core.Mods.Abstractions;

public interface IModService
{
	Task RefreshModsAsync();

	Task SaveSettingsAsync();

	IReadOnlyList<IModInfo> GetActiveMods();
}
