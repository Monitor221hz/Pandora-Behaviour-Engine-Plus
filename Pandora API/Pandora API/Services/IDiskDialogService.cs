// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿namespace Pandora.API.Services;

public interface IDiskDialogService
{
    Task<DirectoryInfo?> OpenFolderAsync(string title);
    Task<FileInfo?> OpenFileAsync(string title, params string[] patterns);
}
