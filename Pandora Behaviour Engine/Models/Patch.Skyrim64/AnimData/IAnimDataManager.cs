// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.IO;
using Pandora.API.Patch.Skyrim64;

namespace Pandora.API.Patch.Skyrim64.AnimData;

public interface IAnimDataManager
{
	int NumClipIDs { get; }
	FileInfo OutputAnimDataSingleFile { get; }
	FileInfo TemplateAnimDataSingleFile { get; }

	int GetNextValidID();
	void MergeAnimDataSingleFile();
	void SetOutputPath(DirectoryInfo outputMeshFolder);
	void SplitAnimDataSingleFile(IProjectManager projectManager);
}
