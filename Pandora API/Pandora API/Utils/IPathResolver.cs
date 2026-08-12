// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

namespace Pandora.API.Utils;

public interface IPathResolver
{
    DirectoryInfo GetCurrentFolder();
    DirectoryInfo GetGameDataFolder();
    DirectoryInfo GetTemplateFolder();
    DirectoryInfo GetOutputFolder();
    DirectoryInfo GetOutputMeshFolder();
    DirectoryInfo GetPandoraEngineFolder();
    DirectoryInfo GetAssemblyFolder();
    FileInfo GetActiveModsFile();
    FileInfo GetPreviousOutputFile();
    void SetGameDataFolder(DirectoryInfo gameDataFolder);
    void SetOutputFolder(DirectoryInfo outputFolder);
    void SavePathsConfiguration();
}
