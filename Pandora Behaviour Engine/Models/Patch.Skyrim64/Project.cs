// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using HKX2E;
using Pandora.API.Patch.Skyrim64;
using Pandora.API.Patch.Skyrim64.AnimData;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

namespace Pandora.Models.Patch.Skyrim64;

public class Project : IEquatable<Project>, IProject
{
	public bool Equals(Project? other)
	{
		return other != null && ProjectFile.Equals(other.ProjectFile);
	}

	public override int GetHashCode()
	{
		return ProjectFile.GetHashCode();
	}

	private Dictionary<string, IPackFile> filesByName { get; set; } = [];

	public string Identifier { get; private set; } = string.Empty;

	public bool Valid { get; private set; }

	public IPackFile ProjectFile { get; private set; }

	/// <summary>
	/// Sibling projects can only be two way one to one at most - if this ever changes in a skyrim update this property must be changed.
	/// </summary>
	public IProject? Sibling { get; set; }

	public IProject? GetSibling() => Sibling as IProject;

	public DirectoryInfo? ProjectDirectory => ProjectFile?.InputHandle.Directory;

	public IPackFileCharacter CharacterPackFile { get; private set; }

	public IPackFileSkeleton SkeletonFile { get; private set; }
	public IPackFileGraph BehaviorFile { get; private set; }
	public IProjectAnimData? AnimData { get; set; }

	public Project()
	{
		Valid = false;
	}

	public Project(IPackFile projectFile)
	{
		Valid = false;
		ProjectFile = projectFile;
		Identifier = Path.GetFileNameWithoutExtension(ProjectFile.InputHandle.Name);
	}

	public Project(
		IPackFile projectfile,
		IPackFileCharacter characterfile,
		IPackFileSkeleton skeletonfile,
		IPackFileGraph behaviorfile
	)
	{
		ProjectFile = projectfile;
		CharacterPackFile = characterfile;
		SkeletonFile = skeletonfile;
		BehaviorFile = behaviorfile;

		Identifier = Path.GetFileNameWithoutExtension(ProjectFile.InputHandle.Name);
		Valid = true;
	}

	public IPackFile LookupPackFile(string name) => filesByName[name];

	public bool TryLookupPackFile(string name, [NotNullWhen(true)] out IPackFile? packFile) =>
		filesByName.TryGetValue(name, out packFile);

	public bool TryLookupPackFileEx(string name, [NotNullWhen(true)] out IPackFile? packFile)
	{
		packFile = TryLookupPackFile(name, out var exPackFile) ? exPackFile : null;
		return packFile != null;
	}

	public bool ContainsPackFile(string name) => filesByName.ContainsKey(name);

	public List<string> MapFiles(IPackFileCache cache)
	{
		DirectoryInfo? behaviorFolder = BehaviorFile.InputHandle.Directory;
		if (behaviorFolder == null)
			return [];

		var behaviorFiles = behaviorFolder.GetFiles("*.hkx");

		lock (filesByName)
		{
			foreach (var behaviorFile in behaviorFiles)
			{
				var packFile = cache.LoadPackFileGraph(behaviorFile, this);

				//packFile.DeleteExistingOutput();
				filesByName.Add(packFile.Name, packFile);
			}

			if (!filesByName.ContainsKey(SkeletonFile.Name))
				filesByName.Add(SkeletonFile.Name, SkeletonFile);
			if (!filesByName.ContainsKey(CharacterPackFile.Name))
				filesByName.Add(CharacterPackFile.Name, CharacterPackFile);

			filesByName.Add($"{Identifier}_skeleton", SkeletonFile);
			filesByName.Add($"{Identifier}_character", CharacterPackFile);

			//SkeletonFile.DeleteExistingOutput();
			//CharacterFile.DeleteExistingOutput();

			return filesByName.Keys.ToList();
		}
	}

	public static Project Create(IPackFile projectFile, IPackFileCache cache)
	{
		if (!projectFile.InputHandle.Exists)
			return new Project();

		IPackFileCharacter characterFile = GetCharacterFile(projectFile, cache);
		if (!characterFile.InputHandle.Exists)
			return new Project();

		var (skeleton, behavior) = GetSkeletonAndBehaviorFile(projectFile, characterFile, cache);

		IPackFileSkeleton skeletonFile = skeleton;
		if (!skeletonFile.InputHandle.Exists)
			return new Project();

		IPackFileGraph behaviorFile = behavior;
		if (!behaviorFile.InputHandle.Exists)
			return new Project();

		var project = new Project(projectFile, characterFile, skeletonFile, behaviorFile);

		projectFile.ParentProject = project;
		characterFile.ParentProject = project;
		skeletonFile.ParentProject = project;
		behaviorFile.ParentProject = project;

		return project;
	}

	public bool Load(IPackFileCache cache)
	{
		if (!ProjectFile.InputHandle.Exists)
			return false;

		ProjectFile = ProjectFile;
		CharacterPackFile = GetCharacterFile(ProjectFile, cache);
		if (!CharacterPackFile.InputHandle.Exists)
			return false;

		var (skeleton, behavior) = GetSkeletonAndBehaviorFile(
			ProjectFile,
			CharacterPackFile,
			cache
		);

		SkeletonFile = skeleton;
		if (!SkeletonFile.InputHandle.Exists)
			return false;

		BehaviorFile = behavior;
		if (!BehaviorFile.InputHandle.Exists)
			return false;

		ProjectFile.ParentProject = this;
		CharacterPackFile.ParentProject = this;
		SkeletonFile.ParentProject = this;
		BehaviorFile.ParentProject = this;

		return true;
	}

	public static Project Load(FileInfo file, IPackFileCache cache) =>
		Create(cache.LoadPackFile(file), cache);

	//public static Project Load(string projectFilePath) => Load(new PackFile(projectFilePath));

	private static IPackFileCharacter GetCharacterFile(IPackFile projectFile, IPackFileCache cache)
	{
		if (projectFile.Container.namedVariants.Count == 0)
		{
			throw new InvalidDataException(
				$"{nameof(hkRootLevelContainer)} for project file has no named variants in file {projectFile.Name}"
			);
		}
		var projectData = (hkbProjectData)projectFile.Container.namedVariants.First()!.variant!;
		var projectStringData =
			projectData.stringData
			?? throw new InvalidDataException(
				$"{nameof(hkbProjectData)} is has null stringData property."
			);
		string characterFilePath = projectStringData.characterFilenames.First();

		return cache.LoadPackFileCharacter(
			new FileInfo(Path.Combine(projectFile.InputHandle.DirectoryName!, characterFilePath))
		);
	}

	private static (IPackFileSkeleton skeleton, IPackFileGraph behavior) GetSkeletonAndBehaviorFile(
		IPackFile projectFile,
		IPackFileCharacter characterFile,
		IPackFileCache cache
	)
	{
		return (
			cache.LoadPackFileSkeleton(
				new FileInfo(
					Path.Combine(
						projectFile.InputHandle.DirectoryName!,
						characterFile.SkeletonFileName
					)
				)
			),
			cache.LoadPackFileGraph(
				new FileInfo(
					Path.Combine(
						projectFile.InputHandle.DirectoryName!,
						characterFile.BehaviorFileName
					)
				)
			)
		);
	}

	public bool Equals(IProject? other)
	{
		return ProjectFile.Equals(other?.ProjectFile);
	}
}
