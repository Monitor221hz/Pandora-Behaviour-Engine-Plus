// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using DynamicData.Binding;
using Pandora.API.Patch;
using Pandora.Data;
using Pandora.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Pandora.Services;

public interface IModLoader
{
	Task<HashSet<IModInfo>> LoadModsAsync(IEnumerable<IModInfoProvider> providers, IEnumerable<DirectoryInfo> directories);
	Task LoadModsVMAsync(ObservableCollectionExtended<ModInfoViewModel> mods, IEnumerable<DirectoryInfo> directories, IEnumerable<IModInfoProvider> providers);
}