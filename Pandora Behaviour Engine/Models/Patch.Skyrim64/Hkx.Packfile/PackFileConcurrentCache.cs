// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Pandora.API.Patch.Skyrim64;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

public class PackFileConcurrentCache : IPackFileCache
{
	private readonly ConcurrentDictionary<string, IPackFile> _pathMap = new(
		StringComparer.OrdinalIgnoreCase
	);
	private readonly Dictionary<IPackFile, List<IProject>> _sharedPackFileProjectMap = [];

	public PackFileConcurrentCache() { }

	private T GetOrCreatePackFile<T>(FileInfo file, Func<T> creator)
		where T : PackFile => (T)_pathMap.GetOrAdd(file.FullName, _ => creator());

	public IPackFile LoadPackFile(FileInfo file) =>
		GetOrCreatePackFile(file, () => new PackFile(file));

	public IPackFileGraph LoadPackFileGraph(FileInfo file) =>
		GetOrCreatePackFile(file, () => new PackFileGraph(file));

	public IPackFileGraph LoadPackFileGraph(FileInfo file, IProject project) =>
		GetOrCreatePackFile(file, () => new PackFileGraph(file, project));

	public IPackFileCharacter LoadPackFileCharacter(FileInfo file) =>
		GetOrCreatePackFile(file, () => new PackFileCharacter(file));

	public IPackFileCharacter LoadPackFileCharacter(FileInfo file, IProject project) =>
		GetOrCreatePackFile(file, () => new PackFileCharacter(file, project));

	public IPackFileSkeleton LoadPackFileSkeleton(FileInfo file) =>
		GetOrCreatePackFile(file, () => new PackFileSkeleton(file));

	public IPackFileSkeleton LoadPackFileSkeleton(FileInfo file, IProject project) =>
		GetOrCreatePackFile(file, () => new PackFileSkeleton(file, project));

	public bool TryLookupSharedProjects(
		IPackFile packFile,
		[NotNullWhen(true)] out List<IProject>? projects
	) => _sharedPackFileProjectMap.TryGetValue(packFile, out projects);
}
