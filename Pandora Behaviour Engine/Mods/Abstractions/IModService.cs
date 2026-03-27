// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DynamicData;
using Pandora.API.Patch;
using Pandora.ViewModels;

namespace Pandora.Mods.Abstractions;

public interface IModService
{
	IObservable<IChangeSet<ModInfoViewModel>> Connect();

	Task RefreshModsAsync();

	Task SaveSettingsAsync();

	IReadOnlyList<IModInfo> GetActiveMods();
}
