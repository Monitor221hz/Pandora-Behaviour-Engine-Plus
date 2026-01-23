// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Pandora.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pandora.Mods.Abstractions;

public interface IModSettingsService
{
    Task<List<ModSaveEntry>> LoadAsync();

    Task SaveAsync(IEnumerable<ModSaveEntry> entries);
}
