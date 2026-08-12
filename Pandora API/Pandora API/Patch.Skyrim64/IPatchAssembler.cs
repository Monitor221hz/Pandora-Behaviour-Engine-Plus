// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Text;
using Pandora.API.Patch;
using Pandora.API.Patch.Skyrim64.AnimData;
using Pandora.API.Patch.Skyrim64.AnimSetData;

namespace Pandora.API.Patch.Skyrim64;

public interface IPatchAssembler
{
    IAnimDataManager AnimDataManager { get; }
    IAnimSetDataManager AnimSetDataManager { get; }
    IProjectManager ProjectManager { get; }
    Task ApplyNativePatchesAsync(RunOrder order);
    bool ApplyPatches();
    Task<bool> ApplyPatchesAsync();
    void AssembleAnimDataPatch(DirectoryInfo folder);
    void AssembleAnimSetDataPatch(DirectoryInfo directoryInfo);
    void AssemblePatch(IModInfo modInfo);
    void AssemblePatch(IModInfo modInfo, DirectoryInfo folder);
    void AssembleProjectAnimDataPatch(DirectoryInfo folder);
    List<(FileInfo inFile, FileInfo outFile)> GetExportFiles();
    void GetPostMessages(StringBuilder builder);
    Task LoadMetaDataAsync();
    void LoadResources();
    Task LoadResourcesAsync();
    Task MergeAllAnimationDataAsync();
    Task QueueNativePatchesAsync();
    Task SaveMetaDataAsync();
}
