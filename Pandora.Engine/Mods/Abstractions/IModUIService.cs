// SPDX-License-Identifier-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using DynamicData;
using Pandora.Core.Mods.Abstractions;
using Pandora.ViewModels;
using System;

namespace Pandora.Mods.Abstractions;

public interface IModUIService : IModService
{
	IObservable<IChangeSet<ModInfoViewModel>> Connect();
}
