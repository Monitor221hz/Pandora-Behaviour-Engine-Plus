// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.IO;
using System.Linq;

namespace Pandora.Core.Paths.Extensions;

public static class PathExtensions
{
	extension(string)
	{
		public static string operator /(string left, string right) => Path.Join(left, right);
	}

	/// <summary>
	/// Resolves a Windows-style relative path (backslash separators, case-insensitive), as found in
	/// Havok files, template path lists and mod folders, against <paramref name="baseDirectory"/>.
	/// On case-sensitive file systems, each segment that does not exist exactly is matched
	/// ignoring case. Segments from the first one that matches nothing onward are kept as given.
	/// </summary>
	public static FileInfo ResolveFile(this DirectoryInfo baseDirectory, string relativePath) =>
		new(Resolve(baseDirectory, relativePath, lastSegmentIsFile: true));

	/// <inheritdoc cref="ResolveFile"/>
	public static DirectoryInfo ResolveDirectory(
		this DirectoryInfo baseDirectory,
		string relativePath
	) => new(Resolve(baseDirectory, relativePath, lastSegmentIsFile: false));

	/// <summary>
	/// Resolves where to write <paramref name="relativePath"/> under <paramref name="outputDirectory"/>,
	/// reusing the casing of folders and files that already exist in <paramref name="casingReference"/>
	/// (e.g. the game Data folder) and then in the output directory. On case-sensitive file systems this
	/// overwrites an existing file that differs only in case, as Windows does, instead of adding a second one.
	/// </summary>
	public static FileInfo ResolveOutputFile(
		this DirectoryInfo outputDirectory,
		string relativePath,
		DirectoryInfo? casingReference = null
	) =>
		new(ResolveOutput(outputDirectory, relativePath, casingReference, lastSegmentIsFile: true));

	/// <inheritdoc cref="ResolveOutputFile"/>
	public static DirectoryInfo ResolveOutputDirectory(
		this DirectoryInfo outputDirectory,
		string relativePath,
		DirectoryInfo? casingReference = null
	) =>
		new(
			ResolveOutput(outputDirectory, relativePath, casingReference, lastSegmentIsFile: false)
		);

	private static string ResolveOutput(
		DirectoryInfo outputDirectory,
		string relativePath,
		DirectoryInfo? casingReference,
		bool lastSegmentIsFile
	)
	{
		if (casingReference is not null)
		{
			relativePath = Path.GetRelativePath(
				casingReference.FullName,
				Resolve(casingReference, relativePath, lastSegmentIsFile)
			);
		}
		return Resolve(outputDirectory, relativePath, lastSegmentIsFile);
	}

	private static string Resolve(
		DirectoryInfo baseDirectory,
		string relativePath,
		bool lastSegmentIsFile
	)
	{
		var segments = relativePath.Split(['\\', '/'], StringSplitOptions.RemoveEmptyEntries);
		string exact = Path.Join([baseDirectory.FullName, .. segments]);
		if (lastSegmentIsFile ? File.Exists(exact) : Directory.Exists(exact))
			return exact;

		string current = baseDirectory.FullName;
		for (int i = 0; i < segments.Length; i++)
		{
			bool isFile = lastSegmentIsFile && i == segments.Length - 1;
			string candidate = Path.Join(current, segments[i]);
			if (!(isFile ? File.Exists(candidate) : Directory.Exists(candidate)))
			{
				var match = Directory.Exists(current)
					? (
						isFile
							? Directory.EnumerateFiles(current)
							: Directory.EnumerateDirectories(current)
					).FirstOrDefault(entry =>
						string.Equals(
							Path.GetFileName(entry),
							segments[i],
							StringComparison.OrdinalIgnoreCase
						)
					)
					: null;
				if (match is null)
					return Path.Join([current, .. segments[i..]]);
				candidate = match;
			}
			current = candidate;
		}
		return current;
	}
}
