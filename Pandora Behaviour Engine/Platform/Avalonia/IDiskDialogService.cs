// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using System.IO;
using System.Threading.Tasks;

namespace Pandora.Platform.Avalonia;

public interface IDiskDialogService
{
    Task<DirectoryInfo?> OpenFolderAsync(string title, DirectoryInfo? initialDirectory = null);
    Task<FileInfo?> OpenFileAsync(string title, DirectoryInfo? initialDirectory = null, params string[] patterns);
}
