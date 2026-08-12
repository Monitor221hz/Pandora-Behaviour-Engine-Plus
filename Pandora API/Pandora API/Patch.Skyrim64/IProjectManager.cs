// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Pandora.API.Patch.Skyrim64;

public interface IProjectManager
{
    HashSet<IPackFile> ActivePackFiles { get; }

    bool ApplyPatches();
    bool ApplyPatchesParallel();
    void GetAnimationInfo(StringBuilder builder);
    void GetExportInfo(StringBuilder builder);
    void GetFNISInfo(StringBuilder builder);
    DirectoryInfo GetOutputDirectory();
    IProject? LoadProject(string projectFilePath);
    IProject? LoadProjectHeader(string projectFilePath);
    void LoadProjects(List<string> projectPaths);
    void LoadProjectsParallel(List<string> projectPaths);
    void LoadTrackedProjects();
    IProject LookupProject(string name);
    bool ProjectExists(string name);
    bool ProjectLoaded(string name);
    bool TryActivatePackFile(IPackFile packFile);
    bool TryActivatePackFilePriority(string name, IProject project, out IPackFile? packFile);
    bool TryActivatePackFilePriority(string name, out IPackFile? packFile);
    bool TryGetProject(string name, [NotNullWhen(true)] out IProject? project);
    bool TryLoadOutputPackFile<T>(IPackFile packFile, [NotNullWhen(true)] out T? outPackFile)
        where T : class, IPackFile;
    bool TryLoadOutputPackFile<T>(
        IPackFile packFile,
        string extension,
        [NotNullWhen(true)] out T? outPackFile
    )
        where T : class, IPackFile;
    bool TryLookupPackFile(string name, out IPackFile? packFile);
    bool TryLookupPackFile(string projectName, string packFileName, out IPackFile? packFile);
    bool TryLookupProjectFolder(string folderName, out IProject? project);
}
