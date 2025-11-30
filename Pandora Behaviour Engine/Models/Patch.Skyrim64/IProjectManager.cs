// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Pandora.Models.Patch.Skyrim64;

public interface IProjectManager
{
	HashSet<IPackFile> ActivePackFiles { get; }

	bool ApplyPatches();
	bool ApplyPatchesParallel();
	void GetAnimationInfo(StringBuilder builder);
	void GetExportInfo(StringBuilder builder);
	void GetFNISInfo(StringBuilder builder);
	DirectoryInfo GetOutputDirectory();
	Project? LoadProject(string projectFilePath);
	IProject? LoadProjectEx(string projectFilePath);
	Project? LoadProjectHeader(string projectFilePath);
	IProject? LoadProjectHeaderEx(string projectFilePath);
	void LoadProjects(List<string> projectPaths);
	void LoadProjectsParallel(List<string> projectPaths);
	void LoadTrackedProjects();
	IProject LookupProject(string name);
	bool ProjectExists(string name);
	bool ProjectLoaded(string name);
	void SetOutputPath(DirectoryInfo baseDirectory);
	bool TryActivatePackFile(PackFile packFile);
	bool TryActivatePackFileEx(IPackFile packFile);
	bool TryActivatePackFilePriority(string name, IProject project, out IPackFile? packFile);
	bool TryActivatePackFilePriority(string name, out IPackFile? packFile);
	bool TryGetProject(string name, [NotNullWhen(true)] out Project? project);
	bool TryGetProjectEx(string name, [NotNullWhen(true)] out IProject? project);
	bool TryLoadOutputPackFile<T>(IPackFile packFile, [NotNullWhen(true)] out T? outPackFile) where T : class, IPackFile;
	bool TryLoadOutputPackFile<T>(IPackFile packFile, string extension, [NotNullWhen(true)] out T? outPackFile) where T : class, IPackFile;
	bool TryLookupPackFile(string name, out IPackFile? packFile);
	bool TryLookupPackFile(string projectName, string packFileName, out IPackFile? packFile);
	bool TryLookupPackFileEx(string name, out IPackFile? packFile);
	bool TryLookupPackFileEx(string name, string packFileName, out IPackFile? packFile);
	bool TryLookupProjectFolder(string folderName, out IProject? project);
	bool TryLookupProjectFolderEx(string folderName, out IProject? project);
}