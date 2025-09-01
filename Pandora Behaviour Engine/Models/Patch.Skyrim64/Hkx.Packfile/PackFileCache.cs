// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

public class PackFileCache : IPackFileCache
{
	private readonly Dictionary<string, PackFile> _pathMap = new(StringComparer.OrdinalIgnoreCase);
	private readonly Dictionary<PackFile, List<Project>> _sharedPackFileProjectMap = [];
	public PackFileCache() { }

	private T GetOrCreatePackFile<T>(FileInfo file, Func<T> creator) where T : PackFile
	{
		lock (_pathMap)
		{
			if (!_pathMap.TryGetValue(file.FullName, out var packFile))
			{
				packFile = creator();
				_pathMap.Add(file.FullName, packFile);
			}
			return (T)packFile;
		}
	}

	public PackFile LoadPackFile(FileInfo file) => 
		GetOrCreatePackFile(file, () => new PackFile(file));


	public PackFileGraph LoadPackFileGraph(FileInfo file) => 
		GetOrCreatePackFile(file, () => new PackFileGraph(file));

	public PackFileGraph LoadPackFileGraph(FileInfo file, Project project) => 
		GetOrCreatePackFile(file, () => new PackFileGraph(file, project));


	public PackFileCharacter LoadPackFileCharacter(FileInfo file) => 
		GetOrCreatePackFile(file, () => new PackFileCharacter(file));

	public PackFileCharacter LoadPackFileCharacter(FileInfo file, Project project) => 
		GetOrCreatePackFile(file, () => new PackFileCharacter(file, project));


	public PackFileSkeleton LoadPackFileSkeleton(FileInfo file) => 
		GetOrCreatePackFile(file, () => new PackFileSkeleton(file));

	public PackFileSkeleton LoadPackFileSkeleton(FileInfo file, Project project) => 
		GetOrCreatePackFile(file, () => new PackFileSkeleton(file, project));


	public bool TryLookupSharedProjects(PackFile packFile, [NotNullWhen(true)] out List<Project>? projects) => 
		_sharedPackFileProjectMap.TryGetValue(packFile, out projects);
}
